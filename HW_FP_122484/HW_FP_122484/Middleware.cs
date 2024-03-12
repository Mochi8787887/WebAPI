using HW_FP_122484.Models;
using HW_FP_122484.Services;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HW_FP_122484
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public readonly ClaimAccessor _claimAccessor;

        private readonly ILog _logger;

        //Middleware處理HTTP的請求和回應
        public Middleware(RequestDelegate next, ClaimAccessor claimAccessor)
        {
            _next = next;
            _logger = LogManager.GetLogger(typeof(Middleware));

            _claimAccessor = claimAccessor;

        }

        //接收HttpContext的資訊
        public async Task Invoke(HttpContext context)
        {

            var request = context.Request;
            //var httpMethod = request.Method;                // API的使用方式
            //var apiPath = request.Path;                     // API的URL
            //var queryString = request.QueryString;          
            //var ip = context.Connection.RemoteIpAddress;    // ip資訊

            var version = _claimAccessor._settings.Version; // 版本號

            //var requestLog = $"{DateTime.Now},  {httpMethod}: IP={ip} {apiPath}{queryString}, 版本號:{version}";
            var requestLog = $"{DateTime.Now},  {request.Method}: IP={context.Connection.RemoteIpAddress} {request.Path}{request.QueryString}, 版本號:{version}";

            // 寫入log在log4net.config
            _logger.Info(requestLog + "\r\n");

            await _next(context);
        }

    }
}