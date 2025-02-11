namespace Sales.Domain.Shared;

public sealed record FailureDetail
{
    private const string VALIDATION_ERROR = "ValidationError";
    private const string INVALID_INPUT_DATA = "Invalid input data.";

    public string Type { get; init; }
    public string Error { get; init; }
    public string Detail { get; init; }

    public FailureDetail(string type, string error, string detail)
    {
        Type = type;
        Error = error;
        Detail = detail;
    }

    public static FailureDetail ResourceNotFound(string resourceName, string resourceId) => new("ResourceNotFound", $"{resourceName} not found.", $"The {resourceName} with ID '{resourceId}' does not exist in our database.");
    public static FailureDetail DefaultValidationError(string detail) => new(VALIDATION_ERROR, INVALID_INPUT_DATA, detail);
    public static FailureDetail NullValue(string propertyName) => new(VALIDATION_ERROR, INVALID_INPUT_DATA, $"The {propertyName} field was not provided.");
    public static FailureDetail MaximumLengthExceeded(string propertyName) => new(VALIDATION_ERROR, INVALID_INPUT_DATA, $"The number of characters of {propertyName} field exceeded the maximum.");
    public static FailureDetail NegativeOrZeroValue(string propertyName) => new(VALIDATION_ERROR, INVALID_INPUT_DATA, $"The {propertyName} field must be greater than 0.");
}