using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace BlazorApp_WebApi.JWT
{
    public class GenerateToken
    {
        private IConfiguration _configuration;
        private string? _KeyDirectory;
        private string? _privateKeyFileName;
        public GenerateToken(IConfiguration configuration) 
        {
            _configuration = configuration;

            //Loading from appsetting.json is a bad practice, environment variables is better
            _KeyDirectory = _configuration.GetSection("KeySettings:KeyDirectory").Value;
            _privateKeyFileName = _configuration.GetSection("KeySettings:PrivateKeyFileName").Value;
        }
        public string GenerateJwtToken(Users _user)
        {
            var privateKey = Convert.FromBase64String(File.ReadAllText(Path.Combine(_KeyDirectory,_privateKeyFileName)));

            // Create signing credentials
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKey, out _);
            var signingCredentials = new SigningCredentials
            (
                new RsaSecurityKey(rsa),
                SecurityAlgorithms.RsaSha256
            );

            // We can add more information like Role or which hierarchy the user belong to
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // We can add more information like start_time and end_time
            var token = new JwtSecurityToken(
                issuer: "Vender",
                audience: "Client",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5), // can add more less time depending on the business model
                signingCredentials: signingCredentials
            );

         
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
