using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Core.Services
{
    public interface IUserService
    {
        Task<CustomResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<CustomResponse<UserDto>> GetUserByNameAsync(string userName);
        Task<CustomResponse<NoContentDto>> CreateUserRoles(string userName, string userRole);
    }
}
