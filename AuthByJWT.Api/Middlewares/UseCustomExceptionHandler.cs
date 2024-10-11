using AuthByJWT.Core.DTOs.CustomResponseDto;
using AuthByJWT.Service.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace AuthByJWT.Api.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                //bu method sonlandırıcı methoddur. Yani response oludtuktan sonra
                //bir hata varsa response sona kadar gitmez, sonlandırıcıdan başa döner
                config.Run(async context =>
                {
                    //geri donus tipi json
                    context.Response.ContentType = "application/json";

                    //uygulama uzerinde hataları yakala
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //burada da uygulamanın mı yoksa bizim gönbderdiğimiz hata mı ayırt etmesi için
                    //işlem yaptık. Eger uygulama hatası varsa 500, biz hata yolladıysak yani ClientSideEx ise 400 at
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };

                    //Contexin status codeunu üstte oluscak degere esitledik
                    context.Response.StatusCode = statusCode;

                    //Response mizi olusturduk
                    var response = CustomResponse<NoContentDto>.Fail(exceptionFeature.Error.Message,statusCode);

                    //Bunu geri dönmek için json için serializa etmemiz lazım
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
