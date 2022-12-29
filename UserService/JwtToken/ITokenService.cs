namespace UserService.JwtToken
{
    public interface ITokenService
    {
        public string buildToken(string key, string issuer, IEnumerable<string> audience, string userName);
    }
}
