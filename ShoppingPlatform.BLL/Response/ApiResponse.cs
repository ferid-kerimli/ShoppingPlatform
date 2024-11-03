namespace ShoppingPlatform.BLL.Response;

public class ApiResponse<T> 
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int StatusCode { get; set; }

    public void Success(T data, int statusCode = 200)
    {
        IsSuccess = true;
        Data = data;
        Errors = null;
        StatusCode = statusCode;
    }

    public void Failure(List<string> errors, int statusCode = 500)
    {
        IsSuccess = false;
        Data = default;
        Errors = errors;
        StatusCode = statusCode;
    }

    public void Failure(string error, int statusCode = 500)
    {
        IsSuccess = false;
        Data = default;
        Errors = new List<string> { error };
        StatusCode = statusCode;
    }
}