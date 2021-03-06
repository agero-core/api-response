﻿using Agero.Core.Checker;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#if NET461
using System.Web.Http.Filters;
#elif NETCOREAPP2_1
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Agero.Core.ApiResponse.Filters
{
    /// <summary>Exception filter attribute</summary>
    public class ExceptionHandlingFilterAttribute: ExceptionFilterAttribute
    {
        /// <summary>Constructor</summary>
        /// <param name="responseHandler">Response handler</param>
        public ExceptionHandlingFilterAttribute(IResponseHandler responseHandler)
        {
            Check.ArgumentIsNull(responseHandler, "responseHandler");

            ResponseHandler = responseHandler;
        }

        /// <summary>Response handler</summary>
        public IResponseHandler ResponseHandler { get; }

#if NET461

        /// <summary>Called when exception happens</summary>
        /// <param name="context">HTTP context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public override async Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            Check.ArgumentIsNull(context, "context");

            context.Response = await GetExceptionResponseAsync(context.Request, context.Exception);
        }

        /// <summary>Returns HTTP response for exception</summary>
        /// <param name="request">HTTP request</param>
        /// <param name="exception">Exception</param>
        protected virtual async Task<HttpResponseMessage> GetExceptionResponseAsync(HttpRequestMessage request, Exception exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "request");

            return await ResponseHandler.HandleExceptionAsync(request, exception);
        }

#elif NETCOREAPP2_1

        /// <summary>Called when exception happens</summary>
        /// <param name="context">HTTP context</param>
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            Check.ArgumentIsNull(context, "context");

            context.Result = await GetExceptionResponseAsync(context.HttpContext.Request, context.Exception);
        }

        /// <summary>Returns HTTP response for exception</summary>
        /// <param name="request">HTTP request</param>
        /// <param name="exception">Exception</param>
        protected virtual async Task<ObjectResult> GetExceptionResponseAsync(HttpRequest request, Exception exception)
        {
            Check.ArgumentIsNull(request, "request");
            Check.ArgumentIsNull(exception, "request");

            return await ResponseHandler.HandleExceptionAsync(request, exception);
        }

#endif

    }
}