using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;
using SmartRecruitmentPlatform.Backend.Repositories.Admin.Implementation;
using SmartRecruitmentPlatform.Backend.Repositories.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Admin.Implementation;
using SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobMatching;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(
        "Backend/appsettings.json",
        optional: false,
        reloadOnChange: true)
    .AddJsonFile(
        "Backend/appsettings.Development.json",
        optional: true,
        reloadOnChange: true)
    .AddEnvironmentVariables();

// Authentication
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

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

builder.Services.AddScoped<IAdminService, AdminService>();

// Add services to the container.
// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Controllers
builder.Services.AddControllers();

// Swagger
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

// Member 4 Matching Weights
builder.Services.Configure<MatchingWeightOptions>(
    builder.Configuration.GetSection("Member4MatchingWeights")
);

// Job Matching Repositories
builder.Services.AddSingleton<IJobRepository, DemoJobRepository>();
builder.Services.AddSingleton<IJobSeekerProfileRepository, DemoJobSeekerProfileRepository>();
builder.Services.AddSingleton<IApplicationRepository, JsonApplicationRepository>();

// Job Matching Services
builder.Services.AddScoped<IMatchScoreService, MatchScoreService>();
builder.Services.AddScoped<IJobMatchingService, JobMatchingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Frontend static files
var frontendPath = Path.Combine(
    app.Environment.ContentRootPath,
    "Frontend"
);

app.UseAuthentication();
app.UseAuthorization();
if (Directory.Exists(frontendPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(frontendPath),
        RequestPath = ""
    });
}

app.MapControllers();

// Open Swagger when opening localhost
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();