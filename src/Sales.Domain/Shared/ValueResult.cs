namespace Sales.Domain.Shared;

public class ValueResult
{
    public bool Succeeded { get; init; }
    public bool Failed { get { return !Succeeded; } }
    public IReadOnlyList<FailureDetail> FailureDetails { get; init; }

    protected ValueResult(bool succeeded, IReadOnlyList<FailureDetail>? failureDetails)
    {
        Succeeded = succeeded;
        FailureDetails = failureDetails ?? [];
    }

    public static ValueResult Success() => new(true, null);
    public static ValueResult Failure(IReadOnlyList<FailureDetail> failureDetails) => new(false, failureDetails);

}

public class ValueResult<TValue> where TValue : class
{
    public bool Succeeded { get; init; }
    public bool Failed { get { return !Succeeded; } }
    public IReadOnlyList<FailureDetail> FailureDetails { get; init; }
    public TValue? Value { get; init; }

    protected ValueResult(bool succeeded, TValue? value, IReadOnlyList<FailureDetail>? failureDetails)
    {
        Succeeded = succeeded;
        Value = value;
        FailureDetails = failureDetails ?? [];
    }

    public static ValueResult<TValue> Success(TValue value) => new(true, value, null);
    public static ValueResult<TValue> Failure(IReadOnlyList<FailureDetail> failureDetails) => new(false, null, failureDetails);
}