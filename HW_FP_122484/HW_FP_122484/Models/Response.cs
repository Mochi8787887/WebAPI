using System;

namespace HW_FP_122484.Models
{
    public class Response
    {
        public object Result { get; set; }
        public string Message { get; set; }
        public string ReturnCode { get; set; }
        public bool Success { get; set; }

        public Response(object result)
        {
            Result = result;
            Message = "成功";
            ReturnCode = "Success";
            Success = true;
        }

        public Response(Exception GG)
        {
            Message = $"失敗： {GG.Message}";
            ReturnCode = "Exception";
            Success = false;
        }

    }
}