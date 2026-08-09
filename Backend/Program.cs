using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Backend/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Backend/appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["JwtSettings:Key"];
        var issuer = builder.Configuration["JwtSettings:Issuer"];
        var audience = builder.Configuration["JwtSettings:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token here."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

