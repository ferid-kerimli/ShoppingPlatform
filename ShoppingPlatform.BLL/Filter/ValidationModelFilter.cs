using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingPlatform.BLL.Response;

namespace ShoppingPlatform.BLL.Filter;

public class ValidationModelFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var response = new ApiResponse<List<string>>();

        var errorList = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        response.Failure(errorList, 400);

        context.Result = new BadRequestObjectResult(response);
    }
}