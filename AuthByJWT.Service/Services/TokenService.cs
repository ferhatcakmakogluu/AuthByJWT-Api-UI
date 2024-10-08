using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Core.Entities;
using AuthByJWT.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomTokenOptionsDto _customTokenOptionsDto;

        public TokenService(UserManager<User> userManager, 
            IOptions<CustomTokenOptionsDto> customTokenOptionsDto)
        {
            _userManager = userManager;
            _customTokenOptionsDto = customTokenOptionsDto.Value;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private async Task<IEnumerable<Claim>> GetClaims(User userApp, List<String> audiences)
        {
            var userRoles = await _userManager.GetRolesAsync(userApp);
            //["admin","manager"]...

            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
                new Claim(ClaimTypes.Name, userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), 
                new Claim("Birth-Date", userApp.BirthDate.ToString())
            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            return userList;
        }


        public TokenDto CreateToken(User user)
        {
            //appsetting.json daki verileri alıp token (access, refresh) omurlerini belirledik
            var accesTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptionsDto.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptionsDto.RefreshTokenExpiration);

            //appsetting.json daki securitykey verisi ile esledik
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptionsDto.SecurityKey);

            //imza olustur, security key ile istedigin algoritmayı secebilirsin
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
               issuer: _customTokenOptionsDto.Issuer,
               expires: accesTokenExpiration,
               notBefore: DateTime.Now,
               claims: GetClaims(user, _customTokenOptionsDto.Audience).Result,
               signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accesTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }
    }
}
