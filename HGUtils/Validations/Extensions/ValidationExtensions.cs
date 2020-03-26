using HGUtils.Common.Enums;
using HGUtils.Exceptions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace HGUtils.Validations.Extensions
{
    public static class ValidationExtensions
    {
        public static void GenerateError(this ActionExecutingContext context)
        {
            var modelStateErrors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors);

            var errorList = modelStateErrors.ToErrorItemViewModel();

            if (!errorList.Any())
            {
                errorList = new List<ErrorItemViewModel>
                {
                    new ErrorItemViewModel
                    {
                        Code = (int)ResultType.BadRequest,
                        Reason = "La petición no cumple con las validaciones predefinidas"
                    }
                };
            }

            context.Result = new JsonResult(new ErrorViewModel
            {
                Errors = errorList
            })
            {
                StatusCode = 400
            };
        }

        private static IEnumerable<ErrorItemViewModel> ToErrorItemViewModel(this IEnumerable<ModelError> errors)
        {
            if (!errors.Any()) yield break;

            foreach (var error in errors)
            {
                yield return new ErrorItemViewModel
                {
                    Code = (int)ResultType.BadRequest,
                    Reason = error.ErrorMessage
                };
            }
        }
    }
}
