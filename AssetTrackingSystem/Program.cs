using AssetTrackingSystem.Data;
using AssetTrackingSystem.Middleware;
using AssetTrackingSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Session servisi
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8); // 8 saat timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Entity Framework DbContext'i ekle
builder.Services.AddDbContext<AssetTrackingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dashboard servisi
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Rapor servisi
builder.Services.AddScoped<IReportService, ReportService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

// Auth middleware'ini ekleyin (ROUTE'LARDAN ÖNCE EKLENMELÝ)
app.UseMiddleware<BasicAuthMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"); // PROGRAMIN BAÞLANGIÇ ADIMI BURASI
app.Run();