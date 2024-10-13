using AuthByJWT.Repository;
using AuthByJWT.Web.Services;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AuthByJWT.Web.AutofacModules;
using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthByJWT.Service.Services;
using AuthByJWT.Web.Middlewares;
using AuthByJWT.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);



/*builder.Services.Configure<CustomTokenOptionsDto>(builder.Configuration.GetSection("TokenOption"));
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptionsDto>();

builder.Services.AddCustomTokenAuth(tokenOptions);*/

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>
    (containerBuilder => containerBuilder.RegisterModule(new AutofacModule()));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Güvenlik amaçlý
    options.Cookie.IsEssential = true; // GDPR uyumluluðu
});
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), opt =>
    {
        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});


builder.Services.AddHttpClient<AuthenticationApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});



var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

if (!app.Environment.IsDevelopment())
{
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

//app.UseCustomException();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
