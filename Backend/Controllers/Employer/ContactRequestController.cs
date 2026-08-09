using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

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
            var request =
                await _contactRequestService.SendAsync(dto);

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