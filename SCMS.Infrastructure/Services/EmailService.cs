// File: SCMS.Infrastructure/Services/EmailService.cs
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace SCMS.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        // Tiêm IConfiguration để đọc appsettings.json
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hàm helper để gửi email, dùng chung cho các hàm khác
        private async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            // Lấy thông tin cấu hình từ appsettings.json
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var password = _configuration["EmailSettings:Password"];

            // Tạo đối tượng email
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(senderName, senderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

            // Gửi email bằng MailKit
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(senderEmail, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var subject = "Yêu cầu đặt lại mật khẩu cho Smart Canteen";
            var body = $@"
                <p>Chào bạn,</p>
                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Vui lòng nhấn vào đường dẫn dưới đây để đặt lại mật khẩu:</p>
                <p><a href='{resetLink}'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không yêu cầu, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,<br>Đội ngũ Smart Canteen</p>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string temporaryPassword)
        {
            var subject = "Chào mừng bạn đến với Smart Canteen";
            var body = $@"
                <p>Chào bạn,</p>
                <p>Tài khoản của bạn tại hệ thống Smart Canteen đã được tạo thành công.</p>
                <p>Bạn có thể đăng nhập bằng email của mình và mật khẩu tạm thời dưới đây:</p>
                <p><b>Mật khẩu:</b> {temporaryPassword}</p>
                <p><i>Lưu ý: Vì lý do bảo mật, bạn sẽ được yêu cầu đổi mật khẩu ngay trong lần đăng nhập đầu tiên.</i></p>
                <p>Trân trọng,<br>Đội ngũ Smart Canteen</p>";

            await SendEmailAsync(toEmail, subject, body);
        }
        public async Task SendParentLinkNotificationAsync(string parentEmail, string studentName)
        {
            var subject = "Thông báo liên kết tài khoản - Smart Canteen";
            var body = $@"
            <p>Xin chào,</p>
            <p>Tài khoản học sinh <b>{studentName}</b> vừa thực hiện liên kết đến tài khoản phụ huynh của bạn trên hệ thống Smart Canteen.</p>
            <p>Từ bây giờ, bạn có thể quản lý ví và theo dõi hoạt động của học sinh trong bảng điều khiển của mình.</p>
            <p>Nếu bạn cho rằng đây là một sự nhầm lẫn, vui lòng liên hệ với quản trị viên nhà trường.</p>
            <p>Trân trọng,<br>Hệ thống Smart Canteen</p>";

            await SendEmailAsync(parentEmail, subject, body);
        }
    }
}