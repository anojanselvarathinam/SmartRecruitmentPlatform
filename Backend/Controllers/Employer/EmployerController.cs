using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpGet("{employerId}")]
        public async Task<IActionResult> GetEmployer(int employerId)
        {
            var employer = await _employerService.GetByIdAsync(employerId);

            if (employer == null)
            {
                return NotFound("Employer not found.");
            }

            return Ok(employer);
        }
    }
}