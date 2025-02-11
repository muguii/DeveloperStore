using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sales.API.Shared;
using Sales.Domain.Shared;

namespace Sales.API.Filters;

internal sealed class ValidationFilter : IActionFilter
{
    // BEFORE
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        List<FailureDetail> errors = context.ModelState.SelectMany(ms => ms.Value!.Errors).Select(e => FailureDetail.DefaultValidationError(e.ErrorMessage)).ToList();
        context.Result = new BadRequestObjectResult(ApiResponse<List<FailureDetail>>.Failure(errors));
    }

    // AFTER
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}