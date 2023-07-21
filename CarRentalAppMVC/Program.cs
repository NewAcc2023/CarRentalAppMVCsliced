using CarRentalAppMVC.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using LazyCache;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetConnectionString("CarDB");
builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddLazyCache();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.Configure<RazorViewEngineOptions>(o =>
{
	o.ViewLocationFormats.Clear();
	o.ViewLocationFormats.Add("/Pages/Cars/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/Brands/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/CarType/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/DriveTypes/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/EngineTypes/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/GearBoxes/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/Orders/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/Users/{0}" + RazorViewEngine.ViewExtension);
	o.ViewLocationFormats.Add("/Pages/Shared/{0}" + RazorViewEngine.ViewExtension);
});


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
