using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Core.DTOs.CustomResponseDto
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; } = new List<String>();


        public ErrorDto(string error)
        {
            Errors.Add(error);
        }
        public ErrorDto(List<String> errors)
        {
            Errors = errors;
        }
    }
}
