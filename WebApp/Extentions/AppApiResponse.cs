using System.Net;
using System.Text.Json.Serialization;

namespace WebApp.Extentions
{
    public class HhcmApiResponse<TData>
    {
        public HhcmApiResponse()
        {

        }

        [JsonConstructor]
        public HhcmApiResponse(string message, TData data)
        {
            Message = message;
            Data = data;
        }

        [JsonPropertyName("message")]
        public string Message { get; set; }


        [JsonPropertyName("data")]
        public TData Data { get; set; }

        public int StatusCode { get; set; }

        public static HhcmApiResponse<TData> Success(TData data, string message)
        {
            return new HhcmApiResponse<TData>
            {
                Data = data,
                Message = message
            };
        }

        public static HhcmApiResponse<TData> Success(string message)
        {
            return new HhcmApiResponse<TData>
            {
                Data = default,
                Message = message,
                StatusCode = 200
            };
        }


        public static HhcmApiResponse<TData> Success(TData data)
        {
            return new HhcmApiResponse<TData>
            {
                Data = data,
                Message = "",
                StatusCode = 200
            };
        }

        public static HhcmApiResponse<TData> Failed(string message, HttpStatusCode httpStatusCode)
        {
            return new HhcmApiResponse<TData>
            {
                Data = default,
                Message = message,
                StatusCode = (int)httpStatusCode
            };
        }
    }
}
