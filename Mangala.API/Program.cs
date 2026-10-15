using Mangala.API;
using Mangala.API.Extensions;
using Mangala.Core.Interfaces;
using Mangala.Core.Services;
using Mangala.Core.Settings;
using Mangala.Data;
using Mangala.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithAuth();
builder.Services.AddAppRateLimiting();

var google = builder.Configuration.GetSection("Authentication:Google");

builder.Services.AddAuthentication()
    .AddCookie()
    .AddGoogle(options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ClientId = google["ClientId"]!;
        options.ClientSecret = google["ClientSecret"]!;
        options.CallbackPath = "/signin-google";
    });

var brevoSettings = builder.Configuration.GetSection("Brevo").Get<BrevoSettings>()
    ?? throw new InvalidOperationException("Brevo settings are missing");
builder.Services.AddSingleton(brevoSettings);
builder.Services.AddHttpClient<IEmailService, EmailService>();

var bachsSettings = builder.Configuration.GetSection("Bachs").Get<BachsSettings>()
    ?? throw new InvalidOperationException("Bachs settings are missing");
builder.Services.AddSingleton(bachsSettings);
builder.Services.AddHttpClient<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
