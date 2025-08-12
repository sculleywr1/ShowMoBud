using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShowMoBudAPI.Contexts;
using ShowMoBudAPI.Services;
using ShowMoBudAPI.Services.Interfaces;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/showmobud_log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//add Serilog for logging
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddDbContext<ShowMoBudContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//custom services:
builder.Services.AddScoped<INewsletterService, NewsletterService>();

//adding service for JWT authentication and authorization
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });
var allowedOrigin = builder.Configuration["FrontendOrigin"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());

    options.AddPolicy("ProdOnly", policy =>
        policy.WithOrigins(allowedOrigin)   // single origin or .WithOrigins("https://app1.com","https://app2.com")
              .AllowAnyHeader()
              .AllowAnyMethod());
});



try
{
    Log.Information("Starting ShowMoBudAPI...");
    var app = builder.Build();



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Use CORS policy
    if (app.Environment.IsDevelopment())
        app.UseCors("AllowAll");
    else
        app.UseCors("ProdOnly");

    //ensure that the authentication middleware is added before the authorization middleware for proper JWT handling and CORS support
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
