using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using SmartRecruitmentPlatform.Backend.Data;

using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;

using SmartRecruitmentPlatform.Backend.Repositories.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Admin.Implementation;

using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;

using SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Admin.Implementation;

using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Implementations;

using SmartRecruitmentPlatform.Backend.Services.JobMatching;


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


// Controllers

builder.Services.AddControllers();


// Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));


// Authentication

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration[
                        "JwtSettings:Issuer"],

                ValidAudience =
                    builder.Configuration[
                        "JwtSettings:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration[
                                "JwtSettings:Key"]!))
            };
    });


// Authentication Services

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IAuthRepository,
    AuthRepository>();


// Employer Services

builder.Services.AddScoped<
    IEmployerService,
    EmployerService>();

builder.Services.AddScoped<
    ICompanyService,
    CompanyService>();
builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces.IApplicationService,
    SmartRecruitmentPlatform.Backend.Services.Employer.Implementations.ApplicationService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces.IContactRequestService,
    SmartRecruitmentPlatform.Backend.Services.Employer.Implementations.ContactRequestService>();

builder.Services.AddScoped<
    IJobService,
    JobService>();


// Employer Repositories

builder.Services.AddScoped<
    IEmployerRepository,
    EmployerRepository>();

builder.Services.AddScoped<
    ICompanyRepository,
    CompanyRepository>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces.IAdminService,
    SmartRecruitmentPlatform.Backend.Services.Admin.Implementation.AdminService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Repositories.Interfaces.IApplicationRepository,
    SmartRecruitmentPlatform.Backend.Repositories.Implementations.ApplicationRepository>();

builder.Services.AddScoped<
    IContactRequestRepository,
    ContactRequestRepository>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Repositories.Interfaces.IJobRepository,
    JobRepository>();


// Admin

builder.Services.AddScoped<
    IAdminService,
    AdminService>();

builder.Services.AddScoped<
    IAdminRepository,
    AdminRepository>();


// Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type =
                Microsoft.OpenApi.Models.SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In =
                Microsoft.OpenApi.Models.ParameterLocation.Header,

            Description =
                "Enter JWT token like: Bearer {your token}"
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type =
                            Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,

                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});


// Job Matching Configuration

builder.Services.Configure<MatchingWeightOptions>(
    builder.Configuration.GetSection(
        "Member4MatchingWeights"));


// Job Matching Repositories

builder.Services.AddSingleton<
    SmartRecruitmentPlatform.Backend.Repositories.JobMatching.IJobRepository,
    DemoJobRepository>();

builder.Services.AddSingleton<
    IJobSeekerProfileRepository,
    DemoJobSeekerProfileRepository>();

builder.Services.AddSingleton<
    SmartRecruitmentPlatform.Backend.Repositories.JobMatching.IApplicationRepository,
    JsonApplicationRepository>();


// Job Matching Services

builder.Services.AddScoped<
    IMatchScoreService,
    MatchScoreService>();

builder.Services.AddScoped<
    IJobMatchingService,
    JobMatchingService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.JobMatching.IApplicationService,
    SmartRecruitmentPlatform.Backend.Services.JobMatching.ApplicationService>();


// Build Application

var app = builder.Build();


// Swagger

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// Frontend

var frontendPath = Path.Combine(
    app.Environment.ContentRootPath,
    "Frontend");

if (Directory.Exists(frontendPath))
{
    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider =
                new PhysicalFileProvider(
                    frontendPath),

            RequestPath = ""
        });
}


// Authentication

app.UseAuthentication();

app.UseAuthorization();


// Controllers

app.MapControllers();


// Run

app.Run();