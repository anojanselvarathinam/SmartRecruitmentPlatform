using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;
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

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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