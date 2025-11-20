namespace Gympt.Services
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? ApiErrorContent { get; }

        public ApiException(string message, int statusCode, string? apiErrorContent) : base(message)
        {
            StatusCode = statusCode;
            ApiErrorContent = apiErrorContent;
        }
    }
}