using Agero.Core.Checker;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Agero.Core.ApiResponse.Helpers;
using Agero.Core.ApiResponse.Models;

namespace Agero.Core.ApiResponse.Extensions
{
    /// <summary>Http request message extensions</summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>Converts HTTP request message to HTTP request information</summary>
        public static async Task<HttpRequestInfo> ToHttpRequestInfoAsync(this HttpRequestMessage request)
        {
            Check.ArgumentIsNull(request, "request");

            var content = await request.Content.ReadAsStringAsync();

            return
                new HttpRequestInfo
                (
                    method: request.Method.ToString(),
                    url: request.RequestUri,
                    body: JsonHelper.MaskPassword(content),
                    headers:
                    request.Headers
                        .Select(h => new HttpHeaderInfo(h.Key, h.Value.ToArray()))
                        .ToArray()
                );
        }
    }
}
