using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _contactRequestService;

        public ContactRequestController(
            IContactRequestService contactRequestService)
        {
            _contactRequestService = contactRequestService;
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> SendContactRequest(
    [FromBody] SendContactRequestDto dto)
        {
            if (!TryGetEmployerId(out int employerId))
            {
                return Unauthorized("Employer ID not found in token.");
            }

            var request =
                await _contactRequestService.SendAsync(
                    employerId,
                    dto);

            return Ok(request);
        }

        [HttpGet("employer/{employerId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetEmployerRequests(
            int employerId)
        {
            if (!TryGetEmployerId(out int authenticatedEmployerId) ||
                authenticatedEmployerId != employerId)
            {
                return Forbid();
            }

            var requests =
                await _contactRequestService
                    .GetByEmployerIdAsync(employerId);

            return Ok(requests);
        }

        [HttpGet("{contactRequestId:int}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetContactRequest(
            int contactRequestId)
        {
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var request =
                await _contactRequestService
                    .GetByIdAsync(contactRequestId, employerId);

            if (request == null)
            {
                return NotFound("Contact request not found.");
            }

            return Ok(request);
        }

        [HttpGet("jobseeker")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetJobSeekerRequests()
        {
            if (!TryGetJobSeekerProfileId(out int profileId))
                return Unauthorized("Job seeker profile ID not found in token.");

            var requests = await _contactRequestService
                .GetByJobSeekerIdAsync(profileId);

            return Ok(requests);
        }

        [HttpPut("{contactRequestId:int}/accept")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Accept(int contactRequestId)
        {
            return await UpdateJobSeekerStatus(
                contactRequestId,
                new UpdateContactStatusDto { Status = "Accepted" });
        }

        [HttpPut("{contactRequestId:int}/decline")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Decline(int contactRequestId)
        {
            return await UpdateJobSeekerStatus(
                contactRequestId,
                new UpdateContactStatusDto { Status = "Declined" });
        }

        private async Task<IActionResult> UpdateJobSeekerStatus(
            int contactRequestId,
            UpdateContactStatusDto dto)
        {
            if (!TryGetJobSeekerProfileId(out int profileId))
                return Unauthorized("Job seeker profile ID not found in token.");

            var request = await _contactRequestService.UpdateStatusAsync(
                contactRequestId,
                profileId,
                dto);

            if (request == null)
                return NotFound("Pending contact request not found.");

            return Ok(request);
        }

        private bool TryGetEmployerId(out int employerId)
        {
            var employerIdClaim = User.FindFirst("employerId")?.Value;
            return int.TryParse(employerIdClaim, out employerId);
        }

        private bool TryGetJobSeekerProfileId(out int profileId)
        {
            var profileIdClaim = User.FindFirst("jobSeekerProfileId")?.Value;
            return int.TryParse(profileIdClaim, out profileId);
        }
    }
}
