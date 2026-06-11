using API.Erorrs;
using API.Helpers;
using Core;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Service;

namespace API.Extensions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IAuthService), typeof(AuthService)); 
            services.AddAutoMapper(typeof(MappingProfiles)); 


            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                          .SelectMany(p => p.Value.Errors)
                                                          .Select(E => E.ErrorMessage)
                                                          .ToList();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });


            return services;
        }
    }
}
