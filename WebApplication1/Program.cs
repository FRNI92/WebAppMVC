using Business.Services;
using Database.Data;
using Database.Entities;
using Database.Repos;
using IdentityDatabase.Data;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();//to work with notification entitites

//add dbcontext via builder
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));
builder.Services.AddDbContext<IdentityAppContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDB")));
//Identity Register. add your entity and identityrole. addEFCStore and you identityDBcontexxt. configure tokens after <>) x=> {
builder.Services.AddIdentity<AppUserEntity, IdentityRole>(x =>
    {
    x.Password.RequiredLength = 8;
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<IdentityAppContext>()
    .AddDefaultTokenProviders();
// sets up how the cookie will work.
// if not logged in. goes to account/signin
// if not autorized. go to account/denied
// httponly denies js to acces it.
// // isEssential means that cookieconsent does not apply here
//exipration is the lifetime of the cookie
//sliding is extending the life of the cookie each time the use does something
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/account/signin";
    x.AccessDeniedPath = "/Admin/AdminLogin";
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
    //x.Cookie.Expiration = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
});

// register hub notification
builder.Services.AddScoped<NotificationService>();
//register services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<AddressService>();
//register repos
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<ProjectMemberRepository>();
builder.Services.AddScoped<StatusRepository>();
builder.Services.AddScoped<AddressRepository>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

//rolemanagement with seperate identity database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUserEntity>>();

    var adminEmail = "admin@admin.com";
    var adminPassword = "Admin123!";

    string[] roles = new[] { "Administrator", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var existingUser = await userManager.FindByEmailAsync(adminEmail);
    if (existingUser == null)
    {
        var adminUser = new AppUserEntity
        {
            UserName = adminEmail,
            Email = adminEmail
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapHub<NotificationHub>("/notoficationhub");

app.Run();
