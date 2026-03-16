//using LinkMaker.Data;
//using LinkMaker.Data.Interfaces;
//using LinkMaker.Services.Implementations;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using LinkMaker.Data.Entities.Identity;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();
//builder.Services.AddRazorPages();
//// Register your Service and its Interface
//builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddDbContext<LinkMakerDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<LinkMakerIdentityDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("LinkMakerIdentityDbContextConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    //options.SignIn.RequireConfirmedAccount = true;
//    // Password settings
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 8;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = true;
//    options.Password.RequireLowercase = true;
//    // Lockout settings
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.AllowedForNewUsers = true;
//    // User settings
//    options.User.RequireUniqueEmail = true;
//    // Sign-in settings
//    options.SignIn.RequireConfirmedEmail = false;
//    options.SignIn.RequireConfirmedPhoneNumber = false;
//}
//)
//.AddEntityFrameworkStores<LinkMakerIdentityDbContext>()
//.AddDefaultTokenProviders();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Identity/Account/Login";
//    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//});
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(60);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

////builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<LinkMakerIdentityDbContext>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseRouting();

//app.UseAuthorization();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();


//app.Run();
using LinkMaker.Data;
using LinkMaker.Data.Entities.Identity;
using LinkMaker.Data.Interfaces;
using LinkMaker.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LinkMakerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<LinkMakerIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LinkMakerIdentityDbContextConnection")));

builder.Services.AddIdentity<LinkMakerUser, UserManagerRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<LinkMakerIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUrlService, UrlService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserManagerRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<LinkMakerUser>>();

//    string[] roles = { "Admin", "User", "Manager" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new UserManagerRole { Name = role });
//        }
//    }

//    string adminEmail = "admin@linkmaker.com";
//    string adminPassword = "Admin123!";

//    var adminUser = await userManager.FindByEmailAsync(adminEmail);
//    if (adminUser == null)
//    {
//        adminUser = new LinkMakerUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            EmailConfirmed = true
//        };

//        var result = await userManager.CreateAsync(adminUser, adminPassword);
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(adminUser, "Admin");
//        }
//    }
//}
app.Run();