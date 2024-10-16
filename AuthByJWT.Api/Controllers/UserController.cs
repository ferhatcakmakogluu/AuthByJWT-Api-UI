﻿using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthByJWT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreteUser(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUserRoles(UserRoleDto userRoleDto)
        {
            return ActionResultInstance(await _userService.CreateUserRoles(userRoleDto.UserName,userRoleDto.Role));
        }

        [Authorize]
        [HttpGet("Data")]
        public IActionResult Data()
        {
            CustomData customData = new CustomData {Id=1, Name = "Data" };
            return Ok(customData);
        }
    }
}
