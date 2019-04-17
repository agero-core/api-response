using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse
{
    /// <summary>Response handler</summary>
    public interface IResponseHandler
    {
        /// <summary>Handles validation error response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <returns>HTTP response message</returns>
        Task<HttpResponseMessage> HandleValidationErrorsAsync(HttpRequestMessage request, IReadOnlyCollection<string> errors);

        /// <summary>Handles exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Exception</param>
        /// <returns>HTTP response message</returns>
        Task<HttpResponseMessage> HandleExceptionAsync(HttpRequestMessage request, Exception exception);
    }
}