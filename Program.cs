using AspNetCoreHero.ToastNotification;
using HMS.Implementation.Interface;
using HMS.Implementation.Services;
using HotelManagementSystem.Dto.Implementation.Services;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("SMTPConfig"));

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IBookingServices, BookingService>();
builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddTransient<ICustomerServices, CustomerServices>();
builder.Services.AddTransient< IOrderServices, OrderServices>();
builder.Services.AddTransient<IProductServices, ProductServices>();
builder.Services.AddTransient<ICustomerReviewService, CustomerReviewService>();
builder.Services.AddTransient<IAmenityService , AmenityService>();
builder.Services.AddTransient<IPaymentServices , PaymentServices>();
builder.Services.AddTransient<ICustomerStatusServices , CustomerStatusServices>();
builder.Services.AddTransient<IImageService  , ImageService>();
builder.Services.AddTransient<IFileService  , FileService>();
builder.Services.ConfigureApplicationCookie(config => config.LoginPath = "/Login");
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
}
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
