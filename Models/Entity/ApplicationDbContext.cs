using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Model.Entity
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed an admin user
            var adminRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN" 
                },
                 new IdentityRole
                 {
                     Id = Guid.NewGuid().ToString(),
                     Name = "Customer",
                     NormalizedName = "Customer"
                 }
            );

            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    SecurityStamp = string.Empty,
                    UserRole = UserRole.Admin,
                    Name = "Ahmad Korede",
                    AgeRange = "20-40",
                    Gender = Gender.Male,
                    FirstName = "Ahmad",
                    LastName = "oseni"
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<CustomerReview> CustomerReviews  { get; set; }
        public DbSet<CustomerStatus> CustomerStatuses { get; set; }
       
    }
}
