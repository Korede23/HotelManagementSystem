using AspNetCoreHero.ToastNotification;
using HMS.Implementation.Interface;
using HMS.Implementation.Services;
using HotelManagementSystem.Dto.Implementation.Services;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.IRepository;
using HotelManagementSystem.Implementation.Repository;
using HotelManagementSystem.Implementation.Services;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Use Serilog for Logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("SMTPConfig"));
builder.Services.AddHttpClient<PaystackService>(client =>
{
    client.BaseAddress = new Uri("https://api.paystack.co/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["Paystack:SecretKey"]}");
});


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
builder.Services.AddTransient<IPaystackService , PaystackService>();
builder.Services.AddTransient<ICustomerStatusServices , CustomerStatusServices>();
builder.Services.AddTransient<IImageService  , ImageService>();
builder.Services.AddTransient<IFileService  , FileService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDashBoardService, DashBoardService>();
builder.Services.AddScoped<IDashBoardRepository, DashBoardRepository>();
builder.Services.AddTransient<IRequestPasswordResetService , RequestPasswordResetService>();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
}
);
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/User/Login";
});

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


