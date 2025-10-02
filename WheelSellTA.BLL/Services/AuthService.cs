using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace WheelSellTA.BLL.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO model)
        {
            var user = _mapper.Map<User>(model);
            user.NormalizedEmail = model.Email.ToUpper();
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Saler");
            }

            return result;
        }

        public async Task<string?> LoginUserAsync(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                _logger.LogWarning($"Login failed for email {model.Email}: User not found.");
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning($"Login failed for email {model.Email}: Incorrect password.");
                return null;
            }
            try
            {
                _logger.LogInformation($"Password check SUCCESSFUL for {model.Email}. Attempting token generation...");
                return await GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                // Если генерация токена падает, мы увидим точную причину здесь.
                _logger.LogError(ex, $"TOKEN GENERATION FAILED for user {model.Email}: {ex.Message}");
                return null; 
            }
        }
        
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            
            // 🛑 ПРОВЕРКА 1: SECRET KEY
            var secretKey = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Ошибка конфигурации: ключ 'Secret' в JwtSettings не найден или пуст. Проверьте appsettings.json.");
            }
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // 🛑 ПРОВЕРКА 2: EXPIRY IN DAYS
            var expiryString = jwtSettings["ExpiryInDays"];
            if (string.IsNullOrEmpty(expiryString) || !double.TryParse(expiryString, NumberStyles.Any, CultureInfo.InvariantCulture, out var expiryDays))
            {
                 throw new InvalidOperationException("Ошибка конфигурации: 'ExpiryInDays' в JwtSettings неверен, отсутствует или не является числом.");
            }
            var expires = DateTime.Now.AddDays(expiryDays);

            // 🛑 ПРОВЕРКА 3: ISSUER и AUDIENCE
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("Ошибка конфигурации: 'Issuer' или 'Audience' в JwtSettings отсутствует.");
            }
            
            var token = new JwtSecurityToken(
                issuer: issuer, // Используем переменную
                audience: audience, // Используем переменную
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}