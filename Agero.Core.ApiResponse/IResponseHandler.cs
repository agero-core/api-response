using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if NET461
using System.Net.Http;
#elif NETCOREAPP2_1
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#endif

namespace Agero.Core.ApiResponse
{
    /// <summary>Response handler</summary>
    public interface IResponseHandler
    {
        /// <summary>Handles validation error response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <returns>HTTP response message</returns>
#if NET461
        Task<HttpResponseMessage> HandleValidationErrorsAsync(HttpRequestMessage request, IReadOnlyCollection<string> errors);
#elif NETCOREAPP2_1
        Task<ObjectResult> HandleValidationErrorsAsync(HttpRequest request, IReadOnlyCollection<string> errors);
#endif
        
        /// <summary>Handles exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Exception</param>
        /// <returns>HTTP response message</returns>
#if NET461
        Task<HttpResponseMessage> HandleExceptionAsync(HttpRequestMessage request, Exception exception);
#elif NETCOREAPP2_1
        Task<ObjectResult> HandleExceptionAsync(HttpRequest request, Exception exception);
#endif
    }
}