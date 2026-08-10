using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _contactRequestService;

        public ContactRequestController(
            IContactRequestService contactRequestService)
        {
            _contactRequestService = contactRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactRequest(
    [FromBody] SendContactRequestDto dto)
        {
            var employerIdClaim =
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(employerIdClaim, out int employerId))
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
        public async Task<IActionResult> GetEmployerRequests(
            int employerId)
        {
            var requests =
                await _contactRequestService
                    .GetByEmployerIdAsync(employerId);

            return Ok(requests);
        }

        [HttpGet("{contactRequestId}")]
        public async Task<IActionResult> GetContactRequest(
            int contactRequestId)
        {
            var request =
                await _contactRequestService
                    .GetByIdAsync(contactRequestId);

            if (request == null)
            {
                return NotFound("Contact request not found.");
            }

            return Ok(request);
        }
    }
}