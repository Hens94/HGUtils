using FluentValidation.AspNetCore;
using HGUtils.Validations;
using HGUtils.Validations.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddControllersWithValidations(
            this IServiceCollection services,
            Action<Validator> validators = null,
            Action<MvcOptions> options = null)
        {
            if (!(validators is null)) validators(new Validator(services));

            return services
                .Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)
                .AddControllers(mvcOptions =>
                {
                    mvcOptions.Filters.Add(typeof(RequestValidationAttribute));
                    if (!(options is null)) options(mvcOptions);
                })
                .AddFluentValidation();
        }
    }
}
