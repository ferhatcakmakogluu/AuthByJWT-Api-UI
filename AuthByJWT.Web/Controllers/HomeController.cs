using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthByJWT.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthenticationApiService _authenticationApiService;

        public HomeController(ILogger<HomeController> logger, AuthenticationApiService authenticationApiService)
        {
            _logger = logger;
            _authenticationApiService = authenticationApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Privacy()
        {
            var response = await _authenticationApiService.GetData();
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}
