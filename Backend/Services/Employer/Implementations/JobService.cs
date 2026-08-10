using SmartRecruitmentPlatform.Backend.DTOs.Employer.Job;
using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;

        public JobService(
            IJobRepository jobRepository,
            ICompanyRepository companyRepository)
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
        }

        public async Task<JobResponseDto?> GetByIdAsync(
            int jobId,
            int employerId)
        {
            if (!await IsJobOwnedByEmployerAsync(jobId, employerId))
            {
                return null;
            }

            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
            {
                return null;
            }

            return MapToResponse(job);
        }

        public async Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(
            int companyId,
            int employerId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company == null || company.EmployerId != employerId)
            {
                return new List<JobResponseDto>();
            }

            var jobs = await _jobRepository.GetByCompanyIdAsync(companyId);

            return jobs.Select(MapToResponse);
        }

        public async Task<JobResponseDto> CreateAsync(
            JobCreateDto dto,
            int employerId)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);

            if (company == null || company.EmployerId != employerId)
            {
                throw new InvalidOperationException(
                    "Company not found or access denied.");
            }

            var job = new Job
            {
                CompanyId = dto.CompanyId,
                JobTitle = dto.JobTitle,
                Description = dto.Description,
                RequiredSkills = dto.RequiredSkills,
                RequiredExperience = dto.RequiredExperience,
                Education = dto.Education,
                Location = dto.Location,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _jobRepository.CreateAsync(job);

            return MapToResponse(created);
        }

        public async Task<JobResponseDto?> UpdateAsync(
    int jobId,
    int employerId,
    JobUpdateDto dto)
        {
            if (!await IsJobOwnedByEmployerAsync(jobId, employerId))
            {
                return null;
            }

            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
            {
                return null;
            }

            job.JobTitle = dto.JobTitle;
            job.Description = dto.Description;
            job.RequiredSkills = dto.RequiredSkills;
            job.RequiredExperience = dto.RequiredExperience;
            job.Education = dto.Education;
            job.Location = dto.Location;
            job.IsActive = dto.IsActive;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(job);

            return MapToResponse(job);
        }

        public async Task<bool> CloseJobAsync(
    int jobId,
    int employerId)
        {
            if (!await IsJobOwnedByEmployerAsync(jobId, employerId))
            {
                return false;
            }

            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
            {
                return false;
            }

            job.IsActive = false;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(job);

            return true;
        }

        private async Task<bool> IsJobOwnedByEmployerAsync(
            int jobId,
            int employerId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
            {
                return false;
            }

            var company = await _companyRepository.GetByIdAsync(
                job.CompanyId);

            if (company == null)
            {
                return false;
            }

            return company.EmployerId == employerId;
        }

        private static JobResponseDto MapToResponse(Job job)
        {
            return new JobResponseDto
            {
                JobId = job.JobId,
                CompanyId = job.CompanyId,
                JobTitle = job.JobTitle,
                Description = job.Description,
                RequiredSkills = job.RequiredSkills,
                RequiredExperience = job.RequiredExperience,
                Education = job.Education,
                Location = job.Location,
                IsActive = job.IsActive,
                CreatedAt = job.CreatedAt,
                UpdatedAt = job.UpdatedAt
            };
        }
    }
}
