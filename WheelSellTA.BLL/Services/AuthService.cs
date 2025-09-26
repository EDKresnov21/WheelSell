using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO model)
        {
            var user = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Saler");
            }

            return result;
        }

        public async Task<string> LoginUserAsync(LoginDTO model)
        {
            // Шаг 1: Ищем пользователя по Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                // Пользователь не найден, возвращаем null (для 401)
                return null;
            }

            // Шаг 2: Проверяем пароль. Это самая вероятная точка сбоя.
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            
            if (!passwordCheck)
            {
                // Неверный пароль, возвращаем null (для 401)
                return null;
            }

            // Шаг 4: Если все проверки пройдены, генерируем и возвращаем токен.
            return await GenerateJwtToken(user);
        }
        
        // Логика генерации JWT-токена
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["ExpiryInDays"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}