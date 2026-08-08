using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public interface IMatchScoreService
{
    MatchResultDto Calculate(JobSeekerProfile profile, Job job);
}
