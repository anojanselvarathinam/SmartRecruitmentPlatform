using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;
using System.Security.Claims;

namespace SmartRecruitmentPlatform.Backend.Controllers.JobSeeker;

[ApiController]
[Route("api/jobseeker")]
[Authorize(Roles = "JobSeeker")]
public class JobSeekerController : ControllerBase
{
    private readonly IJobSeekerService _jobSeekerService;
    private readonly ISkillService _skillService;
    private readonly IEducationService _educationService;
    private readonly IExperienceService _experienceService;
    private readonly ICvService _cvService;

    public JobSeekerController(
        IJobSeekerService jobSeekerService,
        ISkillService skillService,
        IEducationService educationService,
        IExperienceService experienceService,
        ICvService cvService)
    {
        _jobSeekerService = jobSeekerService;
        _skillService = skillService;
        _educationService = educationService;
        _experienceService = experienceService;
        _cvService = cvService;
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }


    // =========================
    // PROFILE
    // =========================

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();

        var profile =
            await _jobSeekerService.GetProfileAsync(userId);

        if (profile == null)
        {
            return NotFound(new
            {
                message = "Profile not found."
            });
        }

        return Ok(profile);
    }


    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        UpdateJobSeekerProfileDto dto)
    {
        var userId = GetUserId();

        var profile =
            await _jobSeekerService.UpdateProfileAsync(
                userId,
                dto);

        return Ok(profile);
    }


    // =========================
    // SKILLS
    // =========================

    [HttpGet("skills")]
    public async Task<IActionResult> GetSkills()
    {
        var userId = GetUserId();

        var skills =
            await _skillService.GetSkillsAsync(userId);

        return Ok(skills);
    }


    [HttpPost("skills")]
    public async Task<IActionResult> AddSkill(
        AddSkillDto dto)
    {
        var userId = GetUserId();

        var skill =
            await _skillService.AddSkillAsync(
                userId,
                dto);

        if (skill == null)
        {
            return NotFound();
        }

        return Ok(skill);
    }


    [HttpPut("skills/{skillId}")]
    public async Task<IActionResult> UpdateSkill(
        int skillId,
        AddSkillDto dto)
    {
        var userId = GetUserId();

        var skill =
            await _skillService.UpdateSkillAsync(
                userId,
                skillId,
                dto);

        if (skill == null)
        {
            return NotFound();
        }

        return Ok(skill);
    }


    [HttpDelete("skills/{skillId}")]
    public async Task<IActionResult> DeleteSkill(
        int skillId)
    {
        var userId = GetUserId();

        var result =
            await _skillService.DeleteSkillAsync(
                userId,
                skillId);

        if (!result)
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Skill deleted successfully."
        });
    }


    // =========================
    // EDUCATION
    // =========================

    [HttpGet("education")]
    public async Task<IActionResult> GetEducation()
    {
        var userId = GetUserId();

        var education =
            await _educationService.GetEducationAsync(userId);

        return Ok(education);
    }


    [HttpPost("education")]
    public async Task<IActionResult> AddEducation(
        AddEducationDto dto)
    {
        var userId = GetUserId();

        var education =
            await _educationService.AddEducationAsync(
                userId,
                dto);

        if (education == null)
        {
            return NotFound();
        }

        return Ok(education);
    }


    [HttpPut("education/{educationId}")]
    public async Task<IActionResult> UpdateEducation(
        int educationId,
        AddEducationDto dto)
    {
        var userId = GetUserId();

        var education =
            await _educationService.UpdateEducationAsync(
                userId,
                educationId,
                dto);

        if (education == null)
        {
            return NotFound();
        }

        return Ok(education);
    }


    [HttpDelete("education/{educationId}")]
    public async Task<IActionResult> DeleteEducation(
        int educationId)
    {
        var userId = GetUserId();

        var result =
            await _educationService.DeleteEducationAsync(
                userId,
                educationId);

        if (!result)
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Education deleted successfully."
        });
    }


    // =========================
    // EXPERIENCE
    // =========================

    [HttpGet("experience")]
    public async Task<IActionResult> GetExperiences()
    {
        var userId = GetUserId();

        var experiences =
            await _experienceService.GetExperiencesAsync(userId);

        return Ok(experiences);
    }


    [HttpPost("experience")]
    public async Task<IActionResult> AddExperience(
        AddExperienceDto dto)
    {
        var userId = GetUserId();

        var experience =
            await _experienceService.AddExperienceAsync(
                userId,
                dto);

        if (experience == null)
        {
            return NotFound();
        }

        return Ok(experience);
    }


    [HttpPut("experience/{experienceId}")]
    public async Task<IActionResult> UpdateExperience(
        int experienceId,
        AddExperienceDto dto)
    {
        var userId = GetUserId();

        var experience =
            await _experienceService.UpdateExperienceAsync(
                userId,
                experienceId,
                dto);

        if (experience == null)
        {
            return NotFound();
        }

        return Ok(experience);
    }


    [HttpDelete("experience/{experienceId}")]
    public async Task<IActionResult> DeleteExperience(
        int experienceId)
    {
        var userId = GetUserId();

        var result =
            await _experienceService.DeleteExperienceAsync(
                userId,
                experienceId);

        if (!result)
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "Experience deleted successfully."
        });
    }


    // =========================
    // CV
    // =========================

    [HttpGet("cv")]
    public async Task<IActionResult> GetCvs()
    {
        var userId = GetUserId();

        var cvs =
            await _cvService.GetCvDocumentsAsync(userId);

        return Ok(cvs);
    }


    [HttpPost("cv")]
    public async Task<IActionResult> UploadCv(
        IFormFile file)
    {
        var userId = GetUserId();

        var cv =
            await _cvService.UploadCvAsync(
                userId,
                file);

        if (cv == null)
        {
            return BadRequest(new
            {
                message = "CV upload failed."
            });
        }

        return Ok(cv);
    }


    [HttpDelete("cv/{cvId}")]
    public async Task<IActionResult> DeleteCv(
        int cvId)
    {
        var userId = GetUserId();

        var result =
            await _cvService.DeleteCvAsync(
                userId,
                cvId);

        if (!result)
        {
            return NotFound();
        }

        return Ok(new
        {
            message = "CV deleted successfully."
        });
    }


    // =========================
    // SKILL GAP
    // =========================

    [HttpGet("skill-gap/{jobId}")]
    public async Task<IActionResult> GetSkillGap(
        int jobId,
        [FromServices] SmartRecruitmentPlatform.Backend.Services.JobMatching.IJobMatchingService jobMatchingService)
    {
        var userId = GetUserId();

        var details = await jobMatchingService.GetDetailsAsync(userId, jobId);

        if (details == null)
        {
            return NotFound(new
            {
                message = "Job not found."
            });
        }

        var skillGap = new SkillGapDto
        {
            JobId = jobId,
            MatchScore = details.Match.TotalScore,
            MissingSkills = details.Match.MissingSkills
        };

        return Ok(skillGap);
    }
}
