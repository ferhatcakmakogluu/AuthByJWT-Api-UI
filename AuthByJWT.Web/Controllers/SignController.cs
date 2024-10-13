using AuthByJWT.Core.DTOs;
using AuthByJWT.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthByJWT.Web.Controllers
{
    public class SignController : Controller
    {
        private readonly AuthenticationApiService _authenticationApiService;

        public SignController(AuthenticationApiService authenticationApiService)
        {
            _authenticationApiService = authenticationApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            var response = await _authenticationApiService.CreateToken(loginDto);
            if (response != null)
            {
                HttpContext.Session.SetString("token", response.AccessToken.ToString());
                return RedirectToAction("Index", "Home");
            }

            return BadRequest("kullanıcı bulunamadı");
        }


        [Authorize]
        public IActionResult Success()
        {
            return View();
        }
    }
}
