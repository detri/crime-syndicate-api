using CrimeSyndicate.DbContexts;
using CrimeSyndicate.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Internal;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.

services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = ConfigurationOptions.Parse(config.GetConnectionString("RedisCache"));
});

services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "csc_session";
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    options.IdleTimeout = TimeSpan.FromDays(1);
});

services.AddSingleton<ITicketStore, RedisTicketStore>();
services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme).Configure<ITicketStore>((options, store) =>
{
    options.SessionStore = store;
});

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = false;
}).AddCookie(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "csc_auth";
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

var dbConString = config.GetConnectionString("CrimeContext");
services.AddDbContext<CrimeContext>(options =>
{
    options.UseNpgsql(dbConString);
});

services.AddControllers();

// Initialize the app

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbCtx = scope.ServiceProvider.GetRequiredService<CrimeContext>();
    dbCtx.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
