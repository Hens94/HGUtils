using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HGUtils.Validations
{
    public class Validator
    {
        private readonly IServiceCollection _services;

        public Validator(IServiceCollection services)
        {
            _services = services;
        }

        public void AddValidator<T, TValidator>()
            where T : class where TValidator : AbstractValidator<T> =>
            _services
                .AddSingleton<IValidator<T>, TValidator>();
    }
}
