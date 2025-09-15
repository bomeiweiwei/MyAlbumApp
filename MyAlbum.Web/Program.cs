using MyAlbum.IoC;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.SysConfigs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1) 設兩套 Cookie 驗證：MemberAuth（前台）、AdminAuth（後台）
//    並用 PolicyScheme 依 URL 路徑自動選擇預設驗證/挑戰的 Scheme
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "AppAuth";
        options.DefaultChallengeScheme = "AppAuth";
    })
    .AddPolicyScheme("AppAuth", "AppAuth", options =>
    {
        options.ForwardDefaultSelector = ctx =>
        {
            var path = ctx.Request.Path;
            // /Admin 底下自動走後台的 Cookie
            return path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase)
                ? "AdminAuth"
                : "MemberAuth";
        };
    })
    .AddCookie("MemberAuth", opt =>
    {
        opt.LoginPath = "/Identity/Login";
        opt.LogoutPath = "/Identity/Logout";
        opt.AccessDeniedPath = "/Identity/Denied";
        opt.Cookie.Name = "memberAuth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 開發用 http 可暫時改 SameAsRequest
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    })
    .AddCookie("AdminAuth", opt =>
    {
        opt.LoginPath = "/Admin/Identity/Login";     // 後台的登入頁
        opt.LogoutPath = "/Admin/Identity/Logout";
        opt.AccessDeniedPath = "/Admin/Identity/Denied";
        opt.Cookie.Name = "adminAuth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// 2) 授權政策：限定各自的 Cookie + UserType
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MemberOnly", policy =>
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes("MemberAuth")
              .RequireClaim("UserType", LoginUserType.Member.GetDescription()));

    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes("AdminAuth")
              .RequireClaim("UserType", LoginUserType.Employee.GetDescription()));
});

// 初始化設定
ConfigurationManager configuration = builder.Configuration;
ConfigManager.Initial(configuration);
// 管理注入
builder.Services.RegisterService(configuration);

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

app.UseAuthentication(); // 若後台需要登入
app.UseAuthorization();

// 3) 後台路由整區強制授權（不寫 [Authorize] 也會被擋）
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization("EmployeeOnly");

// 4) 前台預設路由（無 Area）
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

