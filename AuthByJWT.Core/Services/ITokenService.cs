using AuthByJWT.Core.DTOs.jwtDTOs;
using AuthByJWT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
    }
}
