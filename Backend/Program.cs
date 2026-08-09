using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using SmartRecruitmentPlatform.Backend.Data;

// Auth
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;

// Employer
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation;

using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Implementations;

// Employer aliases
using EmployerJobRepository =
    SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces.IJobRepository;

using EmployerJobRepositoryImplementation =
    SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation.JobRepository;

using EmployerApplicationRepository =
    SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces.IApplicationRepository;

using EmployerApplicationRepositoryImplementation =
    SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation.ApplicationRepository;


// Job Matching
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;
using SmartRecruitmentPlatform.Backend.Services.JobMatching;

// Job Matching aliases
using MatchingJobRepository =
    SmartRecruitmentPlatform.Backend.Repositories.JobMatching.IJobRepository;

using MatchingApplicationRepository =
    SmartRecruitmentPlatform.Backend.Repositories.JobMatching.IApplicationRepository;

using MatchingApplicationService =
    SmartRecruitmentPlatform.Backend.Services.JobMatching.IApplicationService;

using MatchingApplicationServiceImplementation =
    SmartRecruitmentPlatform.Backend.Services.JobMatching.ApplicationService;


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

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();


// Employer Services

builder.Services.AddScoped<
    IEmployerService,
    EmployerService>();

builder.Services.AddScoped<
    ICompanyService,
    CompanyService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Interfaces.IApplicationService,
    SmartRecruitmentPlatform.Backend.Services.Implementations.ApplicationService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Interfaces.IContactRequestService,
    SmartRecruitmentPlatform.Backend.Services.Implementations.ContactRequestService>();

builder.Services.AddScoped<
    SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces.IJobService,
    SmartRecruitmentPlatform.Backend.Services.Employer.Implementations.JobService>();


// Employer Repositories

builder.Services.AddScoped<
    IEmployerRepository,
    EmployerRepository>();

builder.Services.AddScoped<
    ICompanyRepository,
    CompanyRepository>();

builder.Services.AddScoped<
    EmployerJobRepository,
    EmployerJobRepositoryImplementation>();

builder.Services.AddScoped<
    EmployerApplicationRepository,
    EmployerApplicationRepositoryImplementation>();

builder.Services.AddScoped<
    IContactRequestRepository,
    ContactRequestRepository>();


// Job Matching

builder.Services.Configure<MatchingWeightOptions>(
    builder.Configuration.GetSection(
        "Member4MatchingWeights"));


// Job Matching Repositories

builder.Services.AddSingleton<
    MatchingJobRepository,
    DemoJobRepository>();

builder.Services.AddSingleton<
    IJobSeekerProfileRepository,
    DemoJobSeekerProfileRepository>();

builder.Services.AddSingleton<
    MatchingApplicationRepository,
    JsonApplicationRepository>();


// Job Matching Services

builder.Services.AddScoped<
    IMatchScoreService,
    MatchScoreService>();

builder.Services.AddScoped<
    IJobMatchingService,
    JobMatchingService>();

builder.Services.AddScoped<
    MatchingApplicationService,
    MatchingApplicationServiceImplementation>();


// Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// Build Application

var app = builder.Build();


// HTTP Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// Frontend static files

var frontendPath = Path.Combine(
    app.Environment.ContentRootPath,
    "Frontend");

if (Directory.Exists(frontendPath))
{
    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider =
                new PhysicalFileProvider(frontendPath),

            RequestPath = ""
        });
}


app.MapControllers();

app.Run();