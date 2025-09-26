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
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình quan hệ User - Wallet (1-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            // Cấu hình quan hệ tự tham chiếu cho User (Phụ huynh - Học sinh)
            modelBuilder.Entity<User>()
                .HasMany(u => u.LinkedStudents)
                .WithOne(u => u.Parent)
                .HasForeignKey(u => u.ParentId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Cấu hình quan hệ cho Transaction
            modelBuilder.Entity<Transaction>()
               .HasOne(t => t.Wallet)
               .WithMany() // Một ví có nhiều giao dịch
               .HasForeignKey(t => t.WalletId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Student" },
                new Role { RoleId = 2, RoleName = "CanteenStaff" },
                new Role { RoleId = 3, RoleName = "CanteenManager" },
                new Role { RoleId = 4, RoleName = "SystemAdmin" },
                new Role { RoleId = 5, RoleName = "Parent" },
                new Role { RoleId = 6, RoleName = "HeadTeacher" },
                new Role { RoleId = 7, RoleName = "SchoolAdministration" }
            );

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
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 15,
                    FullName = "Default Parent",
                    Email = "parent@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("parent@123"),
                    RoleId = 5 
                },
                new User
                {
                    UserId = 16,
                    FullName = "Default Head Teacher",
                    Email = "headteacher@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("headteacher@123"),
                    RoleId = 6 
                },
                new User
                {
                    UserId = 17,
                    FullName = "School Admin User",
                    Email = "schooladmin@scms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("schooladmin@123"),
                    RoleId = 7 
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