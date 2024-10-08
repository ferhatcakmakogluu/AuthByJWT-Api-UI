using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthByJWT.Core.DTOs.CustomResponseDto;

namespace AuthByJWT.Core.Services
{
    public interface IAuthenticationService
    {
        Task<CustomResponse<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<CustomResponse<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<CustomResponse<NoContentDto>> RevokeRefreshToken(string refreshToken);
    }
}
