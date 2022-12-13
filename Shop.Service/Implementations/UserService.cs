using Data.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Service.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Infrastructure.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Service.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Service.Implementations
{
    public class UserService : IGenericService<User>, IAuthService
    {
        private readonly IUnitOfWork<User> _unitOfWork;
        private readonly IConfiguration _configuration;
        public UserService(IUnitOfWork<User> unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region GenericService
        public async Task<IEnumerable<User>> GetAll()
        {
            var articles = await _unitOfWork.Repository.GetAll();
            return articles;
        }

        public async Task<User> GetById(int id)
        {
            var article = await _unitOfWork.Repository.GetById(id);
            return article;
        }

        public async Task<User> Insert(User entity)
        {
            entity = await _unitOfWork.Repository.Insert(entity);
            await _unitOfWork.Commit();
            return entity;
        }


        public async Task<User> Update(User entity)
        {
            entity = await _unitOfWork.Repository.Update(entity);
            await _unitOfWork.Commit();
            return entity;
        }

        #endregion

        #region AuthService
        public async Task<TokenModelDto> Authorize(string username, string password)
        {
            if (_unitOfWork.Repository is not IUserRepository<User> userRepository)
            {
                throw new Exception("User repository doesn't exist");
            }

            var user = await userRepository.CheckUser(username, password);
            var token = await GenerateTokens(user);

            return token;
        }

        public async Task<TokenModelDto> GenerateRefreshToken(TokenModelDto tokenModel)
        {
            var identity = GetInfoFromAccessToken(tokenModel.AccessToken);

            var userId = identity.Claims.FirstOrDefault(e => e.Type == ClaimConstants.Id)?.Value;

            if (userId == null)
            {
                throw new BadRequestEntityException("Claim id doesn't exist");
            }

            var user = await _unitOfWork.Repository.GetById(Convert.ToInt32(userId));

            if (user.RefreshToken == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
            {
                throw new UnproccesableException("Refresh token isn't valid");
            }

            var token = await GenerateTokens(user);

            return token;
        }

        private async Task<TokenModelDto> GenerateTokens(User user)
        {
            var token = CreateTokens(user);
            var expires = Convert.ToDouble(_configuration["JWT:RefreshTokenExpireInDays"]);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(expires);

            await _unitOfWork.Repository.Update(user);
            await _unitOfWork.Commit();
            return token;
        }

        public async Task<bool> Revoke(int id)
        {
            var user = await _unitOfWork.Repository.GetById(id);
            await RevokeUser(user);
            await _unitOfWork.Commit();

            return true;
        }

        public async Task<bool> RevokeAll()
        {
            var users = await _unitOfWork.Repository.GetAll();

            foreach (var user in users)
            {
                await RevokeUser(user);
            }

            await _unitOfWork.Commit();
            return true;
        }

        #endregion

        #region Private methods for create and validate tokens

        private TokenModelDto CreateTokens(User user)
        {
            var accessToken = CreateAccessToken(user);
            var refreshToken = CreateRefreshToken();
            return new TokenModelDto(accessToken, refreshToken);
        }

        private static string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string CreateAccessToken(User user)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
            var expires = double.Parse(_configuration["JWT:AccessTokenExpireInMinutes"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimConstants.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimConstants.Username, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.Now.AddMinutes(expires),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }


        private async Task RevokeUser(User user)
        {
            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = null;
            await _unitOfWork.Repository.Update(user);
        }

        private ClaimsPrincipal GetInfoFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var identity = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
            {
                throw new BadRequestEntityException("Invalid token");
            }

            return identity;
        }

        #endregion
    }
}
