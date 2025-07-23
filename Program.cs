// using Microsoft.AspNetCore.Http.Features;
// using Microsoft.EntityFrameworkCore;
// using MovieWebsite.Models;
// using Microsoft.Extensions.Logging;
// using Serilog;
// using Serilog.Extensions.Logging;
// using MovieWebsite.Data;

// var builder = WebApplication.CreateBuilder(args);

// // Thiết lập Serilog để ghi log vào file
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information()
//     .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
//     .WriteTo.Console()
//     .WriteTo.File(
//         path: Path.Combine(builder.Environment.WebRootPath, "Logs/app-.log"),
//         rollingInterval: RollingInterval.Day,
//         retainedFileCountLimit: 7)
//     .CreateLogger();

// // Thêm dịch vụ logging với Serilog
// builder.Services.AddLogging(logging =>
// {
//     logging.ClearProviders();
//     logging.AddSerilog();
// });

// // Thêm localization services
// builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// // Thêm dịch vụ cho MVC với hỗ trợ localization
// builder.Services.AddControllersWithViews()
//     .AddViewLocalization()
//     .AddDataAnnotationsLocalization();

// // Thêm DbContext với cấu hình SQLite
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// // Cấu hình giới hạn tải lên file lớn
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.Limits.MaxRequestBodySize = 524_288_000; // 500MB
//     options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
// });

// // Cấu hình FormOptions cho dữ liệu multipart
// builder.Services.Configure<FormOptions>(options =>
// {
//     options.MultipartBodyLengthLimit = 524_288_000;
//     options.MemoryBufferThreshold = 524_288;
//     options.BufferBody = true;
// });

// // Đăng ký IWebHostEnvironment
// builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

// // Thêm HttpClient, Session
// builder.Services.AddHttpClient();
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30);
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });

// // Cấu hình xác thực cookie
// builder.Services.AddAuthentication("CookieAuth")
//     .AddCookie("CookieAuth", options =>
//     {
//         options.LoginPath = "/Account/Login";
//         options.AccessDeniedPath = "/Account/AccessDenied";
//         options.ExpireTimeSpan = TimeSpan.FromDays(7);
//     });

// var app = builder.Build();

// // Cấu hình localization middleware
// var supportedCultures = new[] { "en-US", "vi-VN" };
// var localizationOptions = new RequestLocalizationOptions()
//     .SetDefaultCulture(supportedCultures[0])
//     .AddSupportedCultures(supportedCultures)
//     .AddSupportedUICultures(supportedCultures);

// app.UseRequestLocalization(localizationOptions);

// // Áp dụng migrations tự động trong môi trường phát triển
// if (app.Environment.IsDevelopment())
// {
//     using var scope = app.Services.CreateScope();
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     try
//     {
//         dbContext.Database.Migrate();
//         app.Logger.LogInformation("Đã áp dụng migrations cho cơ sở dữ liệu SQLite.");
//     }
//     catch (Exception ex)
//     {
//         app.Logger.LogError(ex, "Lỗi khi áp dụng migrations cho cơ sở dữ liệu SQLite.");
//     }
// }

// // Cấu hình pipeline xử lý HTTP
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Movie/Error");
//     app.UseHsts();
// }

// // app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();
// app.UseSession();

// // Đảm bảo thư mục Uploads tồn tại
// var uploadsPath = Path.Combine(app.Environment.WebRootPath, "Uploads");
// try
// {
//     Directory.CreateDirectory(uploadsPath);
//     app.Logger.LogInformation($"Thư mục Uploads đã được tạo hoặc xác minh tại: {uploadsPath}");
// }
// catch (Exception ex)
// {
//     app.Logger.LogError(ex, $"Lỗi khi tạo thư mục Uploads tại: {uploadsPath}");
// }

// // Đảm bảo thư mục Logs tồn tại
// var logsPath = Path.Combine(app.Environment.WebRootPath, "Logs");
// try
// {
//     Directory.CreateDirectory(logsPath);
//     app.Logger.LogInformation($"Thư mục Logs đã được tạo hoặc xác minh tại: {logsPath}");
// }
// catch (Exception ex)
// {
//     app.Logger.LogError(ex, $"Lỗi khi tạo thư mục Logs tại: {logsPath}");
// }

// // Cấu hình route mặc định
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Movies}/{action=Index}/{id?}");

// //Route Admin
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Admin}/{action=Index}/{id?}");

// // Đảm bảo đóng Serilog khi ứng dụng dừng
// app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

// app.Run();

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity; // Quan trọng: Thêm namespace này
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieWebsite.Data; // Namespace chứa AppDbContext và ApplicationDbContext
using MovieWebsite.Hubs;
using MovieWebsite.Interfaces;
using MovieWebsite.Models;
using MovieWebsite.Services;
using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// --- Serilog Setup ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine(builder.Environment.WebRootPath, "Logs/app-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7
    )
    .CreateLogger();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog();
});

// --- Localization Setup ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();

// --- DbContext Setup ---

// 1. Đăng ký AppDbContext của bạn (quản lý Movie, Episode, v.v.)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2. Đăng ký ApplicationDbContext cho Identity
// // Sử dụng cùng một chuỗi kết nối, hoặc bạn có thể cấu hình chuỗi kết nối khác cho Identity
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Kestrel and Form Options ---
builder.WebHost.ConfigureKestrel(options =>
{
    // options.Limits.MaxRequestBodySize = 524_288_000; // 500MB
    // options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
    options.Limits.MaxRequestBodySize = 4L * 1024L * 1024L * 1024L; // 4GB
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.Configure<FormOptions>(options =>
{
    // options.MultipartBodyLengthLimit = 524_288_000;
    // options.MemoryBufferThreshold = 524_288;
    // options.BufferBody = true;
    // Cần chỉnh sửa giá trị này lên 4GB
    options.MultipartBodyLengthLimit = 4L * 1024L * 1024L * 1024L; // 4GB
    options.MemoryBufferThreshold = 524_288; // Giữ nguyên hoặc điều chỉnh nếu cần
    options.BufferBody = true;
});

// --- Other Services ---
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//Service thoong baso
// Trong Program.cs
builder.Services.AddScoped<INotificationService, NotificationService>();

// --- ASP.NET Core Identity Configuration ---
// Cấu hình ASP.NET Core Identity
builder
    .Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        // Cấu hình các tùy chọn về mật khẩu, tài khoản, vv.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.RequireUniqueEmail = true; // Đảm bảo mỗi email là duy nhất
    })
    .AddEntityFrameworkStores<AppDbContext>() // Liên kết Identity với ApplicationDbContext của bạn
    .AddDefaultTokenProviders(); // Cần thiết cho các tính năng như reset mật khẩu, xác nhận email

// Cấu hình lại Cookie Authentication để khớp với Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    // Các tùy chọn này đã được đặt trong AddIdentity với AddCookie
    // nhưng bạn có thể ghi đè hoặc cấu hình thêm ở đây nếu cần.
    // Ví dụ:
    options.Cookie.Name = "YourMovieAppAuthCookie"; // Tên cookie tùy chỉnh
    options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
    options.LogoutPath = "/Account/Logout"; // Đường dẫn đến trang đăng xuất
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi bị từ chối truy cập
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn cookie
    options.SlidingExpiration = true; // Gia hạn cookie khi người dùng tương tác
});

// Xóa dòng AddAuthentication cũ
// builder.Services.AddAuthentication("CookieAuth")
//     .AddCookie("CookieAuth", options =>
//     {
//         options.LoginPath = "/Account/Login";
//         options.AccessDeniedPath = "/Account/AccessDenied";
//         options.ExpireTimeSpan = TimeSpan.FromDays(7);
//     });

//lich chieu
builder.Services.AddHostedService<EpisodeReleaseService>();

// --- SignalR Setup ---
builder.Services.AddSignalR();

var app = builder.Build();

// --- Request Localization Middleware ---
var supportedCultures = new[] { "en-US", "vi-VN" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// --- Auto Migration in Development ---
// Chúng ta sẽ áp dụng migration cho cả hai DbContext nếu cần
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        appDbContext.Database.Migrate();
        app.Logger.LogInformation("Đã áp dụng migrations cho AppDbContext.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Lỗi khi áp dụng migrations cho AppDbContext.");
    }

    var applicationDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        applicationDbContext.Database.Migrate();
        app.Logger.LogInformation("Đã áp dụng migrations cho ApplicationDbContext (Identity).");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Lỗi khi áp dụng migrations cho ApplicationDbContext (Identity).");
    }
}

// --- HTTP Request Pipeline Configuration ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Movie/Error"); // Thay đổi controller/action lỗi nếu cần
    app.UseHsts();
}

app.UseHttpsRedirection(); // Có thể bỏ comment nếu bạn muốn enforce HTTPS
app.UseStaticFiles();
app.UseRouting();

// Đảm bảo thứ tự của các middleware này
app.UseAuthentication(); // Xác thực người dùng trước khi phân quyền
app.UseAuthorization();
app.UseSession(); // Session middleware thường đi sau Authentication/Authorization

// --- Directory Creation ---
// Đảm bảo thư mục Uploads tồn tại
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "Uploads");
try
{
    Directory.CreateDirectory(uploadsPath);
    app.Logger.LogInformation($"Thư mục Uploads đã được tạo hoặc xác minh tại: {uploadsPath}");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, $"Lỗi khi tạo thư mục Uploads tại: {uploadsPath}");
}

// Đảm bảo thư mục Logs tồn tại
var logsPath = Path.Combine(app.Environment.WebRootPath, "Logs");
try
{
    Directory.CreateDirectory(logsPath);
    app.Logger.LogInformation($"Thư mục Logs đã được tạo hoặc xác minh tại: {logsPath}");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, $"Lỗi khi tạo thư mục Logs tại: {logsPath}");
}

// --- Endpoint Mapping ---
app.MapHub<WatchPartyHub>("/watchPartyHub"); // Định nghĩa endpoint cho hub


// Map điểm cuối của SignalR hub
app.MapHub<ScheduleHub>("/scheduleHub");


// Route mặc định cho Movies controller
app.MapControllerRoute(name: "default", pattern: "{controller=Movies}/{action=Index}/{id?}");
app.UseMiddleware<MovieWebsite.Middleware.PageVisitCounterMiddleware>();

// Route cho Admin controller (có vẻ bạn có một route admin riêng)
// Lưu ý: Nếu bạn có nhiều route với tên "default", ASP.NET có thể chọn sai.
// Tốt hơn là đặt tên khác cho route này nếu nó không phải là route mặc định cuối cùng.
// Ví dụ:
app.MapControllerRoute(
    name: "admin", // Đặt tên route khác
    pattern: "Admin/{controller=Admin}/{action=Index}/{id?}"
);

// Đảm bảo đóng Serilog khi ứng dụng dừng
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
