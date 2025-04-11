using Business.Services;
using Database.Data;
using IdentityDatabase.Data;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

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
    x.AccessDeniedPath = "account/denied";
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
    x.Cookie.Expiration = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
});

//register services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Projects}/{id?}")
    .WithStaticAssets();


app.Run();
