using Agero.Core.ApiResponse.Extensions;
using Agero.Core.ApiResponse.Exceptions;
using Agero.Core.ApiResponse.Models;
using Agero.Core.Checker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse
{
    /// <summary>Base response handler</summary>
    public abstract class BaseResponseHandler
    {
        /// <summary>Constructor</summary>
        /// <param name="extractAdditionalData">Method which extracts exception additional data. If omitted than default logic is used.</param>
        /// <param name="includeExceptionDetails">Include exception details like stack trace to response?</param>
        protected BaseResponseHandler(Func<Exception, object> extractAdditionalData = null, bool includeExceptionDetails = true)
        {
            ExtractAdditionalData = extractAdditionalData ?? (ex => ex.ExtractAdditionalData());
            IncludeExceptionDetails = includeExceptionDetails;
        }

        /// <summary>Method that extracts exception additional data</summary>
        protected Func<Exception, object> ExtractAdditionalData { get; }

        /// <summary>Include exception details like stack trace to response?</summary>
        public bool IncludeExceptionDetails { get; }

        /// <summary>Create HTTP response message for validation errors</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Response message</param>
        /// <param name="code">Response code</param>
        /// <returns>HTTP response message</returns>
        protected virtual HttpResponseMessage CreateValidationErrorsResponse(HttpRequestMessage request, IReadOnlyCollection<string> errors, 
            HttpStatusCode statusCode, string message, string code)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(errors, "errors");
            Check.Argument(errors.Count > 0, "errors.Count > 0");
            Check.ArgumentIsNullOrWhiteSpace(message, "message");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            var response =
                new ValidationErrorResponse
                (
                    message: message,
                    errors: errors.ToArray(),
                    code: code
                );

            return request.CreateResponse(statusCode, response);
        }

        /// <summary>Create HTTP response message base exception</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Base exception</param>
        /// <returns>HTTP response message</returns>
        protected virtual HttpResponseMessage CreateBaseExceptionResponse(HttpRequestMessage request, BaseException exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            var response = exception.ToInfoResponse();

            return request.CreateResponse(exception.HttpStatusCode, response);
        }

        /// <summary>Create HTTP response message for unexpected exception</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Unexpected exception</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Response message</param>
        /// <param name="code">Response code</param>
        
        /// <returns>HTTP response message</returns>
        protected virtual HttpResponseMessage CreateUnexpectedExceptionResponse(HttpRequestMessage request, Exception exception,
            HttpStatusCode statusCode, string message, string code)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");
            Check.ArgumentIsNullOrWhiteSpace(message, "message");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            var response =
                IncludeExceptionDetails
                    ? exception.ToErrorResponse(message, code, ExtractAdditionalData)
                    : new InfoResponse(message, code);

            return request.CreateResponse(statusCode, response);
        }

        /// <summary>Creates validation errors log data</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <param name="status">HTTP status</param>
        /// <param name="code">Response code</param>
        /// <returns>Data</returns>
        protected virtual async Task<object> CreateValidationErrorsLogDataAsync(HttpRequestMessage request, IReadOnlyCollection<string> errors,
            HttpStatusCode status, string code)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(errors, "errors");
            Check.Argument(errors.Count > 0, "errors.Count > 0");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            return
                new
                {
                    request = await request.ToHttpRequestInfoAsync(),
                    response = new
                    {
                        status = (int) status,
                        code = code,
                        errors = errors
                    }
                };
        }

        /// <summary>Creates exception log data</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Validation errors</param>
        /// <param name="status">HTTP status</param>
        /// <param name="code">Response code</param>
        /// <returns>Data</returns>
        protected virtual async Task<object> CreateExceptionLogDataAsync(HttpRequestMessage request, Exception exception,
            HttpStatusCode status, string code)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            return
                new
                {
                    request = await request.ToHttpRequestInfoAsync(),
                    response = new
                    {
                        status = (int)status,
                        code = code,
                        exception = exception.ToError(ExtractAdditionalData)
                    }
                };
        }
    }
}