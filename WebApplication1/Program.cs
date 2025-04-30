using Business.Services;
using Database.Data;
using Database.Repos;
using IdentityDatabase.Data;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.AdminSetup;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//cookieconscent
// register the settings from cookiepolicyoptons
//choose a few you want to work with byt using lambda. 
builder.Services.Configure<CookiePolicyOptions>(x =>
{
    x.CheckConsentNeeded = context => !context.Request.Cookies.ContainsKey("Consent");// check if user already has given consent, check "Consent"
    x.MinimumSameSitePolicy = SameSiteMode.Lax;// this reroutes you to the page you where at when clicking the sign in/up with 3rd party
});


builder.Services.AddSignalR();//Enables support for SignlR to work with notification in real time

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
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/account/signin";// if not logged in. goes to account/signin
    x.AccessDeniedPath = "/Admin/AdminLogin"; // if not autorized. go to account/denied
    x.Cookie.HttpOnly = true;// httponly denies js to acces it.
    x.Cookie.IsEssential = true;  // isEssential means that cookieconsent does not apply here
    //x.Cookie.Expiration = TimeSpan.FromHours(1); //exipration is the lifetime of the cookie
    x.SlidingExpiration = true; //sliding is extending the life of the cookie each time the use does something

    x.Cookie.SameSite = SameSiteMode.None; //third party setup. is created by third party
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always; //third party setup
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(x =>
    {
        x.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        x.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]; ;
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
builder.Services.AddScoped<NoificationRepository>();
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

//cookieconsent
app.UseCookiePolicy();


app.UseAuthentication();
app.UseAuthorization();

//creates a admin if it cant find one in the Identitydatabase
await DataSeeder.SeedRolesAndAdminAsync(app.Services);

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
