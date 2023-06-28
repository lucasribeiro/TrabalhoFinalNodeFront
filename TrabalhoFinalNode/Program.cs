using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvcCoreTutorial.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = builder.Configuration["Application:LoginPath"];
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});




//whatever services are needed in application will be declared here

//..........................................
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// section for configuring middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

// Index method of home controller will be execute first thing in this application
// it is a default url
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Aparelho}/{action=DisplayAparelho}/{id?}");

app.Run();
