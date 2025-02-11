namespace Sales.API.Shared;

internal class ApiResponse
{
    public const string UnhandledException = "An unhandled exception occurred during the execution of the request.";

    public string Status { get; init; }
    public string Message { get; init; }

    protected ApiResponse(string status, string message)
    {
        Status = status;
        Message = message;
    }

    /// <summary>
    /// Utilizado quando uma requisição é bem-sucedida. 
    /// </summary>
    public static ApiResponse Success(string message = "Operation completed successfully.") => new ApiResponse("success", message);

    /// <summary>
    /// Utilizado quando uma requisição for rejeitada devido a dados ou condições inválidas. 
    /// </summary>
    public static ApiResponse Failure(string message = "Operation could not be completed.") => new ApiResponse("fail", message);

    /// <summary>
    /// Utilizado quando ocorrer um erro inesperado no servidor. 
    /// </summary>
    public static ApiResponse Error(string message = UnhandledException) => new ApiResponse("error", message);
}

internal sealed class ApiResponse<TData> : ApiResponse
{
    public TData Data { get; init; }

    private ApiResponse(TData data, string status, string message) : base(status, message)
    {
        Data = data;
    }

    /// <summary>
    /// Utilizado quando uma requisição é bem-sucedida. 
    /// <para>A propriedade <paramref name="Data" /> conterá os dados de retorno, caso existam.</para>
    /// </summary>
    public static ApiResponse<TData> Success(TData data, string message = "Operation completed successfully.") => new ApiResponse<TData>(data, "success", message);

    /// <summary>
    /// Utilizado quando uma requisição for rejeitada devido a dados ou condições inválidas. 
    /// <para>A propriedade <paramref name="Data" /> conterá um objeto explicando o que deu errado.</para>
    /// </summary>
    public static ApiResponse<TData> Failure(TData data, string message = "Operation could not be completed.") => new ApiResponse<TData>(data, "fail", message);

    /// <summary>
    /// Utilizado quando ocorrer um erro inesperado no servidor. 
    /// <para>A propriedade <paramref name="Data" /> pode ser utilizada para retornar outras informações sobre o erro, como as condições que o causaram, stack traces, etc.</para>
    /// </summary>
    public static ApiResponse<TData> Error(TData data, string message = UnhandledException) => new ApiResponse<TData>(data, "error", UnhandledException);
}