using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Helper.HttpServer;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;

namespace MonsterTradingCardsGame.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;

        public AuthenticationService(IUserRepository userRepository, string jwtSecret, int jwtLifespan)
        {
            _userRepository = userRepository;
            _jwtSecret = jwtSecret;
            _jwtLifespan = jwtLifespan;
        }

        public async Task<(bool success, int code, string message)> Register(User user, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(user.UserCredentials.Username))
            {
                return (false, HttpStatusCode.BAD_REQUEST, "Username is not set.");
            }

            var (existingUser,_) = await _userRepository.GetByUsername(user.UserCredentials.Username);
            if (existingUser != null)
            {
                return (false, HttpStatusCode.CONFLICT, "User already exists");
            }

            return await _userRepository.Create(user, hashedPassword);
        }

        public async Task<(bool success, int code, string token)> Login(string name, string password)
        {
            var (user, hashedPassword) = await _userRepository.GetByUsername(name);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, hashedPassword))
            {
                return (false, HttpStatusCode.UNAUTHORIZED,string.Empty);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {   
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserCredentials.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (true, HttpStatusCode.OK, tokenHandler.WriteToken(token));
        }

        public async Task<bool> AuthorizeAdmin(string token)
        {
            var verifiedRole =  VerifyToken(token).Result;
            if (verifiedRole.verified && verifiedRole.role == RoleEnum.Admin)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AuthorizePlayer(string token)
        {
            var verifiedRole = VerifyToken(token).Result;
            if (verifiedRole.verified && (verifiedRole.role == RoleEnum.Player || verifiedRole.role == RoleEnum.Admin))
            {
                return true;
            }
            return false;
        }

        public async Task<(bool verified, RoleEnum? role)> VerifyToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return (false, null);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;

                var user = await _userRepository.GetById(Guid.Parse(userId));
                if (user == null)
                {
                    return (false, null);
                }

                return (true, user.UserCredentials.Role);
            }
            catch
            {
                return (false, null);
            }
        }

        
    }
}