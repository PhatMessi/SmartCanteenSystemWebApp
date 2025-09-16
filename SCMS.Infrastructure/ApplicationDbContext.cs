// File: SCMS.Infrastructure/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using SCMS.Domain;

namespace SCMS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Định nghĩa các vai trò ---
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Student" },
                new Role { RoleId = 2, RoleName = "CanteenStaff" },
                new Role { RoleId = 3, RoleName = "CanteenManager" },
                new Role { RoleId = 4, RoleName = "SystemAdmin" }
            );

            // --- Tài khoản mẫu (đã có) ---
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "System Administrator",
                    Email = "admin@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin@123"),
                    RoleId = 4
                },
                new User
                {
                    UserId = 2,
                    FullName = "Canteen Manager",
                    Email = "canteenmanager@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("canteenmanager@123"),
                    RoleId = 3
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 6,
                    FullName = "Nhat Dung",
                    Email = "student@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("student@123"),
                    RoleId = 1 
                },
                new User
                {
                    UserId = 7,
                    FullName = "Thien Truong",
                    Email = "staff@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("staff@123"),
                    RoleId = 2 
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Món chính" },
                new Category { CategoryId = 2, CategoryName = "Đồ uống" },
                new Category { CategoryId = 3, CategoryName = "Món ăn vặt" },
                new Category { CategoryId = 4, CategoryName = "Chưa phân loại" }
            );
        }
    }
}