
using DataAccess.Mappers;
using DataAccess.Mappers.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRegistrationMapper, RegistrationMapper>();  
builder.Services.AddScoped<IMemberRepository, MemberRepository>();  

builder.Services.AddDbContext<DbPostgreContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
