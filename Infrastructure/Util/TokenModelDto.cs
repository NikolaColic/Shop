namespace Infrastructure.Util
{
    public class TokenModelDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenModelDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
