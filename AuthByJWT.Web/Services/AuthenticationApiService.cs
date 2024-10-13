using AuthByJWT.Api;
using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Core.DTOs.CustomResponses;
using AuthByJWT.Core.DTOs.jwtDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AuthByJWT.Web.Services
{
    public class AuthenticationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<List<WeatherForecast>> Weather()
        {
            var response = await  _httpClient.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast");
            return response;
        }

        public async Task<CustomData> GetData()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Kullanıcı oturum açmamış veya token geçerli değil.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<CustomData>("User/Data");

            if (response == null)
            {
                throw new HttpRequestException("Veri alınamadı. API'den geçerli bir cevap dönmedi.");
            }

            return response;
        }
    }
}
