using SocialNetworkProject.Core.Application;
using SocialNetworkProject.Infrastructure.Identity;
using SocialNetworkProject.Infrastructure.Persistence;
using SocialNetworkProject.Infrastructure.Shared;
using SocialNetworkProject.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(60);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true; 
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<LoginAuthorize>(); 

builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddIdentityLayerIoc(builder.Configuration);
builder.Services.AddSharedLayerIoc(builder.Configuration);
builder.Services.AddApplicationLayerIoc();

var app = builder.Build();

await app.Services.RunIdentitySeedAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();