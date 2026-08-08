using Microsoft.Extensions.FileProviders;
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;
using SmartRecruitmentPlatform.Backend.Services.JobMatching;

var builder = WebApplication.CreateBuilder(args);

// The original repository keeps appsettings under Backend.
builder.Configuration.AddJsonFile(
    "Backend/appsettings.json",
    optional: false,
    reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MatchingWeightOptions>(
    builder.Configuration.GetSection("Member4MatchingWeights"));

// Self-contained demo data/repositories.
// Replace these with the final EF Core repositories when Members 2/3 DB entities are ready.
builder.Services.AddSingleton<IJobRepository, DemoJobRepository>();
builder.Services.AddSingleton<IJobSeekerProfileRepository, DemoJobSeekerProfileRepository>();
builder.Services.AddSingleton<IApplicationRepository, JsonApplicationRepository>();

builder.Services.AddScoped<IMatchScoreService, MatchScoreService>();
builder.Services.AddScoped<IJobMatchingService, JobMatchingService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

var app = builder.Build();

// Keep Swagger available in this demo so the API can be tested directly.
app.UseSwagger();
app.UseSwaggerUI();

var frontendPath = Path.Combine(app.Environment.ContentRootPath, "Frontend");
if (!Directory.Exists(frontendPath))
{
    throw new DirectoryNotFoundException(
        $"Frontend directory not found: {frontendPath}");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
    RequestPath = ""
});

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/pages/JobMatching/jobs.html"));

app.Run();

