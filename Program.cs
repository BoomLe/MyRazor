
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.Services;
using App.Sercurity;
using App.Sercurity.Requirement;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);


// Add DBcontext Connecting to SQL MyBlogContext

builder.Services.AddEntityFrameworkSqlServer().AddDbContext<MyDbContext>(options=>
{
    var stringConnecting = builder.Configuration.GetConnectionString("MyDbContext");
    options.UseSqlServer(stringConnecting);
    
});

// builder.Services.AddEntityFrameworkSqlServer();

builder.Services.AddDefaultIdentity<MyAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();


// Add IDentity làm việc trên MyDbcontext
// builder.Services.AddIdentity<MyAppUser, IdentityRole>()
// .AddEntityFrameworkStores<MyDbContext>()
// .AddDefaultTokenProviders();


//Add Identity UI thư viên làm việc MyDbcontext:
// builder.Services.AddDefaultIdentity<MyAppUser>()
// .AddEntityFrameworkStores<MyDbContext>()
// .AddDefaultTokenProviders();

//Add Thư viện cho Login-Out
// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});
//ADD MailKit
//Add thư viện MailSetiing làm việc nhớ cài Maikit và Mimekit
// builder.Configuration.GetSection("MailSetting");
// builder.Services.Configure<MailSettings>((builder.Configuration.GetSection("MailSetting")));

builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/Khongtruycap.html";
});


//
// builder.Services.AddAuthentication(options =>
// {
//    options.DefaultScheme = "cookies";
//    options.DefaultChallengeScheme = "oidc";
// })
// .AddCookie("cookies", options =>
// {
//    options.Cookie.Name = "appcookie";
//    options.Cookie.SameSite = SameSiteMode.Strict;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
// })
// .AddOpenIdConnect("oidc", options =>
// {
//    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;

// }).AddGoogle(options =>
// {
//     var addGoogle = builder.Configuration.GetSection("Authentication:Google");
//     options.ClientId = addGoogle["ClientId"];
//     options.ClientSecret = addGoogle["ClientSecret"];
//     options.CallbackPath ="/dang-nhap-tu-google/";
// });

//
// ADD dịch vụ thứ 3 Google
builder.Services.AddAuthentication().AddGoogle(options =>
{
    var addGoogle = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = addGoogle["ClientId"];
    options.ClientSecret = addGoogle["ClientSecret"];
    options.CallbackPath ="/dang-nhap-tu-google/";
});

builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();



//ADD
// builder.Services.AddScoped<AuthenticationStateProvier, ServerAuthenticationStateProvider>();
//ADD Policy Role => để thỏa mảng điều kiện đăng nhập vào website(cấp quyền truy cập User):
builder.Services.AddAuthorization( options=>
{
    options.AddPolicy("AllowEditRole", PolicyBuilder => 
    {
        PolicyBuilder.RequireAuthenticatedUser();
        // PolicyBuilder.RequireRole("Admin");
        // PolicyBuilder.RequireRole("Editor");
        PolicyBuilder.RequireClaim("Members ", " abc");
    });

    options.AddPolicy("IsGenz", PolicyBuilder =>
    {
        PolicyBuilder.RequireAuthenticatedUser();
        PolicyBuilder.Requirements.Add(new GeZRequirement());
    });

     options.AddPolicy("ShowAdminMenu", PolicyBuilder =>
     {
        // PolicyBuilder.RequireAuthenticatedUser();
        PolicyBuilder.RequireRole("Admin");

     });

     options.AddPolicy("Cantupdate", PolicyBuilder =>
    {
        
        PolicyBuilder.Requirements.Add(new ArticleCantupdate());
    });
});

// ADD dịch vụ IAuthorizationHandler cho thư mục Sercurity.Requirements
builder.Services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();





// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//đây là phương thức Cookies sửa lỗi kết nối Google
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
//
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 2 Models cho Identity:
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
/*
CREATE, READ, UPDATE, DELETE (CURD)
 => dotnet aspnet-codegenerator razorpage-m EFWebRazor.models.Article -dc EFWebRazor.models.MyDbContext


Identity:
   - Authentication: Xác định danh tính -> Login, logout ...
   
   -Authorization: Xác thực quyền truy cập
    *Role-based authorization - xác thực quyền theo vai trò
      -Role (vai trò cấp quyền) :
        * Tạo Role:
            -> Index (hiện thị danh sách  quản lý)
            ->Created (cung cấp chức năng tạo ra Role)
            -> Edit (chỉnh sửa quyền hành Roles)
            -> Delete (cấp quyền Roles)
              => tạo file Role trong thư mục /Areas/Admin/Pages/Role
              => lệnh tạo file Role:
                 -> dotnet new page -n Index -o Areas/Admin/Pages/Role -p:n App.Admin.Roles
                 -> dotnet new page -n Created -o Areas/Admin/Pages/Role -p:n App.Admin.Roles
                 -> dotnet new page -n EditUserClaim -o Areas/Admin/Pages/User -p:n App.Admin.User

    
                 [Authorize] - Controller, Action, PageModel --> Đăng nhập
    *Policy-based authorization
    *Claims-based authorization   : Claims => Đặc tính , tính chất dối tượng(user)
    ví dụ :
    - bạn có bằng lái B2(Role) => tức là bạn lái xe 4 bánh  
    - trong bằng lái có Năm sinh, quê quán
    - thì những thông tin trong bằng lái B2 là Claims
    - từ đó bạn lấy bằng lái B2 đi mua rượu hoặc chất kích thích thỏa mã điều kiện đủ 18 tuổi.




    - Quản lý user : Sign Up, User, Role ...
    

/Identity/Account/Login
/Identity/Account/Manage
=> dotnet aspnet-codegenerator identity -dc EFWebRazor.models.MyDbContext

*/
//Download LibMan => tìm thư viên Multiple-select trên https://cdnjs.com/
