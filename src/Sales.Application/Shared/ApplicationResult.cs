using Sales.Domain.Shared;
using System.Net;

namespace Sales.Application.Shared;

public class ApplicationResult : ValueResult
{
    public HttpStatusCode Status { get; init; }

    private ApplicationResult(bool succeeded, IReadOnlyCollection<FailureDetail>? failureDetails, HttpStatusCode status) : base(succeeded, failureDetails)
    {
        Status = status;
    }

    public static new ApplicationResult Success() => new(true, null, HttpStatusCode.OK);
    public static ApplicationResult Created() => new(true, null, HttpStatusCode.Created);
    public static ApplicationResult NoContent() => new(true, null, HttpStatusCode.NoContent);
    public static ApplicationResult BadRequest(FailureDetail failureDetail) => BadRequest([failureDetail]);
    public static ApplicationResult BadRequest(IReadOnlyCollection<FailureDetail> failuresDetail) => new(false, failuresDetail, HttpStatusCode.BadRequest);
    public static ApplicationResult NotFound(FailureDetail failureDetail) => new(false, [failureDetail], HttpStatusCode.NotFound);
}

public sealed class ApplicationResult<TValue> : ValueResult<TValue> where TValue : class
{
    public HttpStatusCode Status { get; init; }

    private ApplicationResult(bool succeeded, TValue? value, IReadOnlyCollection<FailureDetail>? failureDetails, HttpStatusCode status) : base(succeeded, value, failureDetails)
    {
        Status = status;
    }

    public static new ApplicationResult<TValue> Success(TValue value) => new(true, value, null, HttpStatusCode.OK);
    public static ApplicationResult<TValue> Created(TValue value) => new(true, value, null, HttpStatusCode.Created);
    public static ApplicationResult<TValue> NoContent() => new(true, null, null, HttpStatusCode.NoContent);
    public static ApplicationResult<TValue> BadRequest(FailureDetail failureDetail) => BadRequest([failureDetail]);
    public static ApplicationResult<TValue> BadRequest(IReadOnlyCollection<FailureDetail> failuresDetail) => new(false, null, failuresDetail, HttpStatusCode.BadRequest);
    public static ApplicationResult<TValue> NotFound(FailureDetail failureDetail) => new(false, null, [failureDetail], HttpStatusCode.NotFound);
}