using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Core.Entities;
using AuthByJWT.Core.Repositories;
using AuthByJWT.Core.Services;
using AuthByJWT.Core.UnitOfWorks;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<AuthRefreshToken> _authRefreshTokenService;

        public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, 
            IUnitOfWork unitOfWork, IGenericRepository<AuthRefreshToken> authRefreshTokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _authRefreshTokenService = authRefreshTokenService;
        }

        public async Task<CustomResponse<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return CustomResponse<TokenDto>.Fail("Email veya şifre yanlis", 400);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return CustomResponse<TokenDto>.Fail("Email veya şifre yanlis", 400);
            }


            var token = _tokenService.CreateToken(user);

            //check refresh token
            var userRefreshToken = await _authRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _authRefreshTokenService.AddAsync(new AuthRefreshToken
                {
                    UserId = user.Id,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();
            return CustomResponse<TokenDto>.Success(token, 200);
        }

        public async Task<CustomResponse<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _authRefreshTokenService.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return CustomResponse<TokenDto>.Fail("Refresh token not found", 404);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user == null)
            {
                return CustomResponse<TokenDto>.Fail("UserId not found", 404);
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.RefreshToken = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return CustomResponse<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<CustomResponse<NoContentDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _authRefreshTokenService.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return CustomResponse<NoContentDto>.Fail("Refresh token not found", 404);
            }

            _authRefreshTokenService.Delete(existRefreshToken);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentDto>.Success(200);
        }
    }
}
