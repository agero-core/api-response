﻿using Agero.Core.ApiResponse.Exceptions;
using Agero.Core.ApiResponse.Models;
using Agero.Core.Checker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

#if NET461
using System.Net.Http;
#elif NETCOREAPP2_1
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#endif

namespace Agero.Core.ApiResponse
{
    /// <summary>Sync response handler</summary>
    public class ResponseHandler : BaseResponseHandler, IResponseHandler
    {
        /// <summary>Constructor</summary>
        /// <param name="logInfo">Method which creates information log (optional)</param>
        /// <param name="logError">Method which creates error log (optional)</param>
        /// <param name="extractAdditionalData">Method which extracts exception additional data. If omitted than default logic is used.</param>
        /// <param name="includeExceptionDetails">Include exception details like stack trace to response?</param>
        public ResponseHandler(Action<string, object> logInfo = null, Action<string, object> logError = null, 
            Func<Exception, object> extractAdditionalData = null, bool includeExceptionDetails = true)
            : base(extractAdditionalData, includeExceptionDetails)
        {
            LogInfo = logInfo ?? ((m, o) => { });
            LogError = logError ?? ((m, o) => { });
        }

        /// <summary>Method which creates information log</summary>
        protected Action<string, object> LogInfo { get; }

        /// <summary>Method which creates error log</summary>
        protected Action<string, object> LogError { get; }

        /// <summary>Handles validation error response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="errors">Validation errors</param>
        /// <returns>HTTP response message</returns>
#if NET461
        public virtual async Task<HttpResponseMessage> HandleValidationErrorsAsync(HttpRequestMessage request, IReadOnlyCollection<string> errors)
#elif NETCOREAPP2_1
        public virtual async Task<ObjectResult> HandleValidationErrorsAsync(HttpRequest request, IReadOnlyCollection<string> errors)
#endif
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(errors, "errors");
            Check.Argument(errors.Count > 0, "errors.Count > 0");

            const string MESSAGE = ResponseMessage.VALIDATION_ERROR;
            const string CODE = ResponseCode.INVALID_REQUEST;
            const HttpStatusCode STATUS = HttpStatusCode.BadRequest;

            var data = await CreateValidationErrorsLogDataAsync(request, errors, STATUS, CODE);

            LogInfo(MESSAGE, data);

            return CreateValidationErrorsResponse(request, errors, STATUS, MESSAGE, CODE);
        }

        /// <summary>Handles exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Exception</param>
        /// <returns>HTTP response message</returns>
#if NET461
        public virtual async Task<HttpResponseMessage> HandleExceptionAsync(HttpRequestMessage request, Exception exception)
#elif NETCOREAPP2_1
        public virtual async Task<ObjectResult> HandleExceptionAsync(HttpRequest request, Exception exception)
#endif
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
#if NET461
        protected virtual async Task<HttpResponseMessage> HandleBaseExceptionAsync(HttpRequestMessage request, BaseException exception)
#elif NETCOREAPP2_1
        protected virtual async Task<ObjectResult> HandleBaseExceptionAsync(HttpRequest request, BaseException exception)
#endif
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            var data = await CreateExceptionLogDataAsync(request, exception, exception.HttpStatusCode, exception.Code);

            LogInfo(exception.Message, data);

            return CreateBaseExceptionResponse(request, exception);
        }

        /// <summary>Handles unexpected exception response</summary>
        /// <param name="request">HTTP request message</param>
        /// <param name="exception">Unexpected exception</param>
        /// <returns>HTTP response message</returns>
#if NET461
        protected virtual async Task<HttpResponseMessage> HandleUnexpectedExceptionAsync(HttpRequestMessage request, Exception exception)
#elif NETCOREAPP2_1
        protected virtual async Task<ObjectResult> HandleUnexpectedExceptionAsync(HttpRequest request, Exception exception)
#endif
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "exception");

            const string MESSAGE = ResponseMessage.UNEXPECTED_ERROR;
            const string CODE = ResponseCode.UNEXPECTED_ERROR;
            const HttpStatusCode STATUS = HttpStatusCode.InternalServerError;

            var data = await CreateExceptionLogDataAsync(request, exception, STATUS, CODE);

            if (exception is TaskCanceledException || exception is OperationCanceledException) 
                LogInfo(exception.Message, data);
            else
                LogError(MESSAGE, data);

            return CreateUnexpectedExceptionResponse(request, exception, STATUS, MESSAGE, CODE);
        }
    }
}