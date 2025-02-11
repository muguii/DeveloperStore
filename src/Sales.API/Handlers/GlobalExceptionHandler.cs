using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Sales.API.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sales.API.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private readonly IHostEnvironment _environment;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IHostEnvironment environment, ILogger<GlobalExceptionHandler> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, ApiResponse.UnhandledException);

        ProblemDetails problemDetails = CreateProblemDetails(httpContext, exception);
        var json = ToJson(problemDetails);

        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsync(json, cancellationToken).ConfigureAwait(false);

        return true;
    }

    private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    {
        var statusCode = context.Response.StatusCode;
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);

        if (string.IsNullOrEmpty(reasonPhrase))
            reasonPhrase = ApiResponse.UnhandledException;

        var problemDetails = new ProblemDetails
        {
            Title = reasonPhrase,
            Status = statusCode
        };

        if (!_environment.IsDevelopment())
            return problemDetails;

        problemDetails.Detail = exception.ToString();
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["data"] = exception.Data;

        return problemDetails;
    }

    private string ToJson(in ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails, SerializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while serializing the error to JSON.");
        }

        return string.Empty;
    }
}