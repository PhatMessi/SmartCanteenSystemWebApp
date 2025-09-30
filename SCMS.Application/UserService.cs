using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using SCMS.Domain.DTOs;
using SCMS.Domain;
using SCMS.Infrastructure;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using SCMS.Infrastructure.Services;


namespace SCMS.Application
{
    public class LoginResult
    {
        public string? Token { get; set; }
        public bool MustChangePassword { get; set; }
        public bool Success => !string.IsNullOrEmpty(Token) || MustChangePassword;
    }

    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(ApplicationDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<LoginResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || user.Role == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new LoginResult { MustChangePassword = false, Token = null }; 
            }

            if (user.MustChangePassword)
            {
                var tempToken = GenerateJwtToken(user, isTemporary: true);
                return new LoginResult { MustChangePassword = true, Token = tempToken };
            }

            return new LoginResult { Token = GenerateJwtToken(user), MustChangePassword = false };
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            if (user.MustChangePassword)
            {
                user.MustChangePassword = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> CreateUserAsync(CreateUserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return null; 
            }

            var temporaryPassword = GenerateRandomPassword();

            var user = new User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(temporaryPassword),
                RoleId = userDto.RoleId,
                MustChangePassword = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _emailService.SendWelcomeEmailAsync(user.Email, temporaryPassword);

            return user;
        }

        private string GenerateRandomPassword()
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+<,>.?/";
            const string allChars = lowercase + uppercase + digits + specialChars;
            int length = 18;

            var random = new RNGCryptoServiceProvider();
            var password = new char[length];

            password[0] = lowercase[GetRandomInt(random, lowercase.Length)];
            password[1] = uppercase[GetRandomInt(random, uppercase.Length)];
            password[2] = digits[GetRandomInt(random, digits.Length)];
            password[3] = specialChars[GetRandomInt(random, specialChars.Length)];

            for (int i = 4; i < length; i++)
            {
                password[i] = allChars[GetRandomInt(random, allChars.Length)];
            }

            return new string(password.OrderBy(c => GetRandomInt(random, length)).ToArray());
        }

        private int GetRandomInt(RNGCryptoServiceProvider random, int max)
        {
            byte[] randomNumber = new byte[4];
            random.GetBytes(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % max;
        }

        private string GenerateJwtToken(User user, bool isTemporary = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            if (isTemporary)
            {
                claims.Add(new Claim("action", "force_change_password"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role!.RoleName));
            }
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: isTemporary ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<bool> ForgotPasswordAsync(string email) 
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return true;
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var passwordResetToken = Convert.ToBase64String(tokenBytes)
                                            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            user.PasswordResetToken = passwordResetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            var webAppBaseUrl = _configuration["WebAppBaseUrl"];
            var resetLink = $"{webAppBaseUrl}/reset-password?token={passwordResetToken}&email={Uri.EscapeDataString(email)}";

            await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                u => u.Email == dto.Email &&
                     u.PasswordResetToken == dto.Token &&
                     u.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).Include(u => u.Parent).ToListAsync();
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.RoleId = userDto.RoleId;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            // Logic cập nhật phụ huynh cho học sinh (chỉ dành cho Admin)
            var role = await _context.Roles.FindAsync(userDto.RoleId);
            if (role != null && role.RoleName == "Student")
            {
                if (string.IsNullOrWhiteSpace(userDto.ParentEmail))
                {
                    user.ParentId = null; // Xóa liên kết nếu email trống
                }
                else
                {
                    var parent = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.ParentEmail && u.Role.RoleName == "Parent");
                    if (parent != null)
                    {
                        user.ParentId = parent.UserId; // Gán ParentId nếu tìm thấy phụ huynh
                    }
                    else
                    {
                        // Tùy chọn: có thể trả về lỗi nếu không tìm thấy email phụ huynh
                        // return false; 
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ParentLinkDetailsDto?> GetLinkedParentAsync(int studentId)
        {
            var student = await _context.Users
                .Include(u => u.Parent) // Nạp thông tin của Parent
                .FirstOrDefaultAsync(u => u.UserId == studentId);

            if (student?.Parent == null)
            {
                return null;
            }

            return new ParentLinkDetailsDto
            {
                ParentId = student.Parent.UserId,
                FullName = student.Parent.FullName,
                Email = student.Parent.Email
            };
        }

        public async Task<(bool Success, string Message)> LinkParentAsync(int studentId, string parentEmail)
        {
            var student = await _context.Users.FindAsync(studentId);
            if (student.ParentId.HasValue)
            {
                return (false, "Tài khoản này đã được liên kết với một phụ huynh.");
            }

            var parent = await _context.Users
                                       .Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.Email == parentEmail);

            if (parent == null)
            {
                return (false, "Không tìm thấy tài khoản phụ huynh với email này.");
            }

            if (parent.Role.RoleName != "Parent")
            {
                return (false, "Tài khoản này không phải là tài khoản Phụ huynh.");
            }

            student.ParentId = parent.UserId;
            await _context.SaveChangesAsync();
            await _emailService.SendParentLinkNotificationAsync(parent.Email, student.FullName);

            return (true, "Liên kết với phụ huynh thành công.");
        }

        public async Task<(bool Success, string Message)> UnlinkParentAsync(int studentId)
        {
            var student = await _context.Users.FindAsync(studentId);
            if (student == null || !student.ParentId.HasValue)
            {
                return (false, "Tài khoản chưa được liên kết hoặc không tồn tại.");
            }

            student.ParentId = null;
            await _context.SaveChangesAsync();

            return (true, "Hủy liên kết thành công.");
        }
        public async Task<bool> IsParentOfStudentAsync(int parentId, int studentId)
        {
            return await _context.Users
                .AnyAsync(u => u.UserId == studentId && u.ParentId == parentId);
        }

        public async Task<List<User>> GetLinkedStudentsAsync(int parentId)
        {
            return await _context.Users
                .Where(u => u.ParentId == parentId)
                .ToListAsync();
        }
    }
}