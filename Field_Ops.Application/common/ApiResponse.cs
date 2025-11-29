using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Field_Ops.Application.common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(int statusCode, string? message = null, T? data = default)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
              => new(statusCode, message, data);

        public static ApiResponse<T> FailResponse(string message, int statusCode = 400)
            => new(statusCode, message, default);
    }
}
