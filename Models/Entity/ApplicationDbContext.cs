using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Model.Entity
{
    public class ApplicationDbContext : IdentityDbContext<User> // Specify your User class here
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed an admin user
            var adminRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
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
                    NormalizedName = "CUSTOMER"
                }
            );

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Address = "Lagos",
                    PhoneNumber = "09068041575",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    SecurityStamp = string.Empty,
                    UserRole = UserRole.Admin,
                    FullName = "Ahmad Korede",
                    AgeRange = "20-40",
                    Gender = Gender.Male,
                    FirstName = "Ahmad",
                    LastName = "Oseni"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );


            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<CustomerStatus> CustomerStatuses { get; set; }
        public DbSet<RequestPasswordReset> RequestPasswordResets { get; set; }
    }
}
