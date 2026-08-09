using BCrypt.Net;
using SmartRecruitmentPlatform.Backend.DTOs.Authentication;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
                registerDto.Role != "Employer" &&
                registerDto.Role != "Admin")
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
                Role = registerDto.Role
            };

            // Save user
            await _authRepository.AddUserAsync(user);
            await _authRepository.SaveChangesAsync();

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
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

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