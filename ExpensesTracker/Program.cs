
using DataAccess.Helper;
using DataAccess.Helper.Interface;
using DataAccess.Mappers;
using DataAccess.Mappers.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRegistrationMapper, RegistrationMapper>();  
builder.Services.AddScoped<ILoginMapper, LoginMapper>();  
builder.Services.AddScoped<IExpenseMapper, ExpenseMapper>();  
builder.Services.AddScoped<IContactMapper, ContactMapper>();  
builder.Services.AddScoped<IMemberRepository, MemberRepository>();  
builder.Services.AddScoped<IHomeRepository, HomeRepository>();  
builder.Services.AddScoped<IHelper, Helper>();  
builder.Services.AddDbContext<DbPostgreContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnection"))
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = Environment.GetEnvironmentVariable("Issuer"),
        ValidAudience = Environment.GetEnvironmentVariable("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key")))
    };

    options.Events = new JwtBearerEvents
    {
        // ✅ Read token from cookie
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("jwtToken", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },

        // ✅ Redirect instead of 401 (for browser)
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.Redirect("/Home/Login");
            return Task.CompletedTask;
        }
    };
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
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
