using HGUtils.Validations.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HGUtils.Validations.Filters
{
    public sealed class RequestValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) context.GenerateError();
        }
    }
}
