using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthByJWT.Core.DTOs.CustomResponseDto
{
    public class CustomResponse<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; private set; }

        [JsonIgnore]
        public bool IsSuccessfull { get; private set; }

        public static CustomResponse<T> Success(T data, int statusCode)
        {
            return new CustomResponse<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
        }

        public static CustomResponse<T> Success(int statusCode)
        {
            return new CustomResponse<T> { StatusCode = statusCode, IsSuccessfull = true };
        }

        public static CustomResponse<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new CustomResponse<T> { Error = errorDto, StatusCode = statusCode, IsSuccessfull = false };
        }

        public static CustomResponse<T> Fail(string error, int statusCode)
        {
            var errorDto = new ErrorDto(error);
            return new CustomResponse<T> { Error = errorDto, StatusCode = statusCode, IsSuccessfull = false };
        }
    }
}
