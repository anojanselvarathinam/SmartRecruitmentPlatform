using System.Text.Json;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public sealed class JsonApplicationRepository : IApplicationRepository
{
    private readonly string _filePath;
    private readonly object _sync = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public JsonApplicationRepository(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(
            environment.ContentRootPath,
            "Backend",
            "Data");

        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "member4-applications.json");

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public Task<bool> ExistsAsync(
        int jobId,
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            var applications = LoadUnsafe();
            var exists = applications.Any(application =>
                application.JobId == jobId &&
                application.JobSeekerId == jobSeekerId);

            return Task.FromResult(exists);
        }
    }

    public Task<JobApplication> AddAsync(
        JobApplication application,
        CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            var applications = LoadUnsafe();

            if (applications.Any(existing =>
                    existing.JobId == application.JobId &&
                    existing.JobSeekerId == application.JobSeekerId))
            {
                throw new InvalidOperationException(
                    "Duplicate application is not allowed for the same vacancy.");
            }

            application.Id = applications.Count == 0
                ? 1
                : applications.Max(existing => existing.Id) + 1;

            applications.Add(application);
            SaveUnsafe(applications);

            return Task.FromResult(application);
        }
    }

    public Task<IReadOnlyList<JobApplication>> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            IReadOnlyList<JobApplication> result = LoadUnsafe()
                .Where(application => application.JobSeekerId == jobSeekerId)
                .OrderByDescending(application => application.AppliedAtUtc)
                .ToList();

            return Task.FromResult(result);
        }
    }

    private List<JobApplication> LoadUnsafe()
    {
        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<JobApplication>();
        }

        return JsonSerializer.Deserialize<List<JobApplication>>(json, _jsonOptions)
            ?? new List<JobApplication>();
    }

    private void SaveUnsafe(List<JobApplication> applications)
    {
        var json = JsonSerializer.Serialize(applications, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
