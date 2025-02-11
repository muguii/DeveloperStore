using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Shared;
using System.Net;

namespace Sales.API.Shared;

[ApiController]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected readonly ISender Sender;

    public ControllerBase(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult ApplicationResultToActionResult(ApplicationResult applicationResult)
        => this.BuildActionResult(applicationResult.Succeeded, applicationResult.Status, null, applicationResult.FailureDetails);

    protected IActionResult ApplicationResultToActionResult<TValue>(ApplicationResult<TValue> applicationResult) where TValue : class
        => this.ApplicationResultToActionResult(applicationResult, _ => null);

    protected IActionResult ApplicationResultToActionResult<TValue>(ApplicationResult<TValue> applicationResult, Func<ApplicationResult<TValue>, object?> successMapper) where TValue : class
        => this.BuildActionResult(applicationResult.Succeeded, applicationResult.Status, successMapper(applicationResult), applicationResult.FailureDetails);

    private IActionResult BuildActionResult(bool succeeded, HttpStatusCode status, object? successData, object failureData)
    {
        if (succeeded)
        {
            return new ObjectResult(successData is null ? ApiResponse.Success() : ApiResponse<object>.Success(successData))
            {
                StatusCode = (int)status
            };
        }

        return new ObjectResult(ApiResponse<object>.Failure(failureData))
        {
            StatusCode = (int)status
        };
    }
}