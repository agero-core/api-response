#if NETCOREAPP2_1

using Agero.Core.ApiResponse.Helpers;
using Agero.Core.ApiResponse.Models;
using Agero.Core.Checker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse.Extensions
{
    /// <summary>Http request extensions</summary>
    public static class HttpRequestExtensions
    {
        /// <summary>Converts HTTP request message to HTTP request information</summary>
        public static async Task<HttpRequestInfo> ToHttpRequestInfoAsync(this HttpRequest request)
        {
            Check.ArgumentIsNull(request, "request");

            var content = await GetRequestBodyAsync(request);

            return
                new HttpRequestInfo
                (
                    method: request.Method,
                    url: new Uri(request.GetEncodedUrl()),
                    body: JsonHelper.MaskPassword(content),
                    headers:
                        request.Headers
                            .Select(h => new HttpHeaderInfo(h.Key, h.Value.ToArray()))
                            .ToArray()
                );
        }

        private static async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            Check.ArgumentIsNull(request, "request");
            
            var initialPosition = request.Body.Position;

            request.Body.Position = 0;

            var result = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = initialPosition;

            return result;
        }
    }
}

#endif