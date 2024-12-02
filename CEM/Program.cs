using CEM.Components;
using CEM.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

//Ket noi CSDL

IConfigurationRoot configuration = new ConfigurationBuilder()
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("appsettings.json")
                                .Build();

builder.Services.AddDbContext<QlbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));





// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddRadzenComponents();

//Service
builder.Services.AddSingleton<MinIOService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NotificationService>();





////Dinh huong trang dau tien 


//app.Use(async (context, next) =>
//{
//    if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/login"))
//    {
//        context.Response.Redirect("/login");
//        return;
//    }
//    await next.Invoke();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
