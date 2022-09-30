using System.Net;
using System.Text.Json.Serialization;

namespace WebApp.Extentions
{
    public class AppApiResponse<TData>
    {
        public AppApiResponse()
        {

        }

        [JsonConstructor]
        public AppApiResponse(string message, TData data)
        {
            Message = message;
            Data = data;
        }

        [JsonPropertyName("message")]
        public string Message { get; set; }


        [JsonPropertyName("data")]
        public TData Data { get; set; }

        public int StatusCode { get; set; }

        public static AppApiResponse<TData> Success(TData data, string message)
        {
            return new AppApiResponse<TData>
            {
                Data = data,
                Message = message
            };
        }

        public static AppApiResponse<TData> Success(string message)
        {
            return new AppApiResponse<TData>
            {
                Data = default,
                Message = message,
                StatusCode = 200
            };
        }


        public static AppApiResponse<TData> Success(TData data)
        {
            return new AppApiResponse<TData>
            {
                Data = data,
                Message = "",
                StatusCode = 200
            };
        }

        public static AppApiResponse<TData> Failed(string message, HttpStatusCode httpStatusCode)
        {
            return new AppApiResponse<TData>
            {
                Data = default,
                Message = message,
                StatusCode = (int)httpStatusCode
            };
        }
    }
}
