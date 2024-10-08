using AuthByJWT.Core.DTOs;
using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AuthRefreshToken, RefreshTokenDto>().ReverseMap();
        }
    }
}
