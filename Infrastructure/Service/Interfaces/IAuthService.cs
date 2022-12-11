using Infrastructure.Util;

namespace Infrastructure.Service.Interfaces
{
    public interface IAuthService
    {
        Task<TokenModelDto> Authorize(string username, string password);
        Task<TokenModelDto> GenerateRefreshToken(TokenModelDto tokenModel);
        Task<bool> RevokeAll();
        Task<bool> Revoke(int id);
    }
}
