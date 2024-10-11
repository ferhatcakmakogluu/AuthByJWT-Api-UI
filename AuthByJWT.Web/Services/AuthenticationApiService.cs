using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Core.DTOs.CustomResponses;
using AuthByJWT.Core.DTOs.jwtDTOs;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace AuthByJWT.Web.Services
{
    public class AuthenticationApiService
    {
        private readonly HttpClient _httpClient;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenDto> CreateToken(LoginDto login)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/CreateToken", login);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TokenDto>>();

            //var accessToken = responseBody.Data.AccessToken.ToString();
            //_httpContextAccessor.HttpContext.Session.SetString("token", accessToken);

            return responseBody.Data;
        }
    }
}
