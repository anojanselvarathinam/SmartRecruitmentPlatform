using BCrypt.Net;
using SmartRecruitmentPlatform.Backend.DTOs.Authentication;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using EmployerModel = SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        private readonly IConfiguration _configuration;

        public AuthService(
            IAuthRepository authRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // Check whether role is valid
            if (registerDto.Role != "JobSeeker" &&
                registerDto.Role != "Employer")
            {
                return false;
            }
            // Check whether email already exists
            var existingUser = await _authRepository
                .GetUserByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                return false;
            }

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(
                registerDto.Password);

            // Create user
            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = registerDto.Role,
                IsActive = true
            };

            EmployerModel? employer = null;
            JobSeekerProfile? jobSeekerProfile = null;

            if (registerDto.Role == "Employer")
            {
                employer = new EmployerModel
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow
                };
            }

            if (registerDto.Role == "JobSeeker")
            {
                var nameParts = registerDto.FullName.Trim().Split(
                    ' ',
                    2,
                    StringSplitOptions.RemoveEmptyEntries);

                jobSeekerProfile = new JobSeekerProfile
                {
                    FirstName = nameParts.Length > 0
                        ? nameParts[0]
                        : registerDto.FullName,
                    LastName = nameParts.Length > 1
                        ? nameParts[1]
                        : string.Empty,
                    CreatedAt = DateTime.UtcNow
                };
            }

            await _authRepository.RegisterUserAsync(
                user,
                employer,
                jobSeekerProfile);

            return true;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            // Find user by email
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            // Check password
            bool passwordIsCorrect = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash);

            if (passwordIsCorrect == false)
            {
                return null;
            }

            // Get JWT settings
            var key = _configuration["JwtSettings:Key"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = _configuration["JwtSettings:ExpiryMinutes"];

            // Create claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            if (user.Employer != null)
            {
                claims.Add(new Claim(
                    "employerId",
                    user.Employer.EmployerId.ToString()));
            }

            if (user.JobSeekerProfile != null)
            {
                claims.Add(new Claim(
                    "jobSeekerProfileId",
                    user.JobSeekerProfile.Id.ToString()));
            }

            // Create security key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            // Create JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(expiryMinutes)),
                signingCredentials: credentials);

            // Convert token to string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
