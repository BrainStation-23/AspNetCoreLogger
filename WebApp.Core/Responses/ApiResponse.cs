using System.Collections.Generic;
using System.Net;
using WebApp.Common.Serialize;

namespace WebApp.Core.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ApiResponse(int statusCode, string message, IEnumerable<string> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            IsSuccess = Success(statusCode);
            Errors = errors;
        }

        public ApiResponse(object data)
        {
            Data = data;
            StatusCode = (int)HttpStatusCode.OK;
            IsSuccess = true;
        }

        public ApiResponse(int statusCode, object data)
        {
            StatusCode = statusCode;
            Data = data;
            IsSuccess = Success(statusCode);
        }

        public ApiResponse(int statusCode, string message, object data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            IsSuccess = Success(statusCode);
        }

        private bool Success(object value)
        {
            return value.ToString().StartsWith('2');
        }

        public override string ToString()
        {
            return this.ToJson();
            //return JsonConvert.SerializeObject(this);
        }
    }
}
