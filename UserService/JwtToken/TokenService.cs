using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserService.JwtToken
{
    public class TokenService : ITokenService
    {
        private TimeSpan ExpiryDuration = new TimeSpan(20, 30, 0);
        public string buildToken(string key, string issuer, IEnumerable<string> audience, string userName)
        {
            var claims = new List<Claim>
       {
           new Claim(JwtRegisteredClaimNames.UniqueName, userName)
       };
            claims.AddRange(audience.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
