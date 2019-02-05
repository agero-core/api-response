using Agero.Core.ApiResponse.Exceptions;
using Agero.Core.ApiResponse.Models;
using Agero.Core.Checker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse
{
    /// <summary>Async response handler</summary>
    public class AsyncResponseHandler : BaseResponseHandler, IResponseHandler
    {
        /// <summary>Constructor</summary>
        /// <param name="logInfoAsync">Method which creates information log (optional)</param>
        /// <param name="logErrorAsync">Method which creates error log (optional)</param>
        /// <param name="extractAdditionalData">Method which extracts exception additional data. If omitted than default logic is used.</param>
        /// <param name="includeExceptionDetails">Include exception details like stack trace to response?</param>
        public AsyncResponseHandler(Func<string, object, Task> logInfoAsync = null, Func<string, object, Task> logErrorAsync = null,
            Func<Exception, object> extractAdditionalData = null, bool includeExceptionDetails = true)
            : base(extractAdditionalData, includeExceptionDetails)
        {
            LogInfoAsync = logInfoAsync ?? ((m, o) => Task.FromResult(0));
            LogErrorAsync = logErrorAsync ?? ((m, o) => Task.FromResult(0));
        }

        /// <summary>Method which creates information log</summary>
        protected Func<string, object, Task> LogInfoAsync { get; }

        /// <summary>Method which creates error log</summary>
        protected Func<string, object, Task> LogErrorAsync { get; }

        /// <summary>Handles validation error response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <returns>HTTP response message</returns>
        public virtual async Task<HttpResponseMessage> HandleValidationErrorsAsync(HttpRequestMessage request, IReadOnlyCollection<string> errors)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(errors, "errors");
            Check.Argument(errors.Count > 0, "errors.Count > 0");

            const string MESSAGE = ResponseMessage.VALIDATION_ERROR;
            const string CODE = ResponseCode.INVALID_REQUEST;
            const HttpStatusCode STATUS = HttpStatusCode.BadRequest;

            var data = await CreateValidationErrorsLogDataAsync(request, errors, STATUS, CODE);

            await LogInfoAsync(MESSAGE, data);

            return CreateValidationErrorsResponse(request, errors, STATUS, MESSAGE, CODE);
        }

        /// <summary>Handles exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Exception</param>
        /// <returns>HTTP response message</returns>
        public virtual async Task<HttpResponseMessage> HandleExceptionAsync(HttpRequestMessage request, Exception exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            if (exception is BaseException baseException)
                return await HandleBaseExceptionAsync(request, baseException);

            return await HandleUnexpectedExceptionAsync(request, exception);
        }

        /// <summary>Handles base exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Base exception</param>
        /// <returns>HTTP response message</returns>
        protected virtual async Task<HttpResponseMessage> HandleBaseExceptionAsync(HttpRequestMessage request, BaseException exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            var data = await CreateExceptionLogDataAsync(request, exception, exception.HttpStatusCode, exception.Code);

            await LogInfoAsync(exception.Message, data);

            return CreateBaseExceptionResponse(request, exception);
        }

        /// <summary>Handles unexpected exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Unexpected exception</param>
        /// <returns>HTTP response message</returns>
        protected virtual async Task<HttpResponseMessage> HandleUnexpectedExceptionAsync(HttpRequestMessage request, Exception exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            const string MESSAGE = ResponseMessage.UNEXPECTED_ERROR;
            const string CODE = ResponseCode.UNEXPECTED_ERROR;
            const HttpStatusCode STATUS = HttpStatusCode.InternalServerError;

            var data = await CreateExceptionLogDataAsync(request, exception, STATUS, CODE);

            if (exception is TaskCanceledException || exception is OperationCanceledException) 
                await LogInfoAsync(exception.Message, data);
            else 
                await LogErrorAsync(MESSAGE, data);

            return CreateUnexpectedExceptionResponse(request, exception, STATUS, MESSAGE, CODE);
        }
    }
}
