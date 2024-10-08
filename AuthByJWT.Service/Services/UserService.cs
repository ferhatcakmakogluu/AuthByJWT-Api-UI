using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Core.Entities;
using AuthByJWT.Core.Services;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Azure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;


        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<CustomResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponse<UserDto>.Fail(new ErrorDto(errors), 400);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return CustomResponse<UserDto>.Success(userDto, 200);
        }

        public async Task<CustomResponse<NoContentDto>> CreateUserRoles(string userName, string userRole)
        {
            //db de admin ve manager role var mı yok mu
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                await _roleManager.CreateAsync(new() { Name = "superadmin" });
                await _roleManager.CreateAsync(new() { Name = "admin" });
                await _roleManager.CreateAsync(new() { Name = "user" });
            }


            //aynı kullanıcıyı birden fazla aynı rol ataması yapmaz
            var user = await _userManager.FindByNameAsync(userName);
            
            if (user == null)
            {
                return CustomResponse<NoContentDto>.Fail("User not found", 404);
            }

            await _userManager.AddToRoleAsync(user, userRole);

            return CustomResponse<NoContentDto>.Success(201);
        }

        public async Task<CustomResponse<UserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return CustomResponse<UserDto>.Fail("Username not found", 404);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return CustomResponse<UserDto>.Success(userDto, 200);
        }
    }
}
