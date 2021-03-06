﻿using System;
using System.Net;
using Agero.Core.ApiResponse.Models;

namespace Agero.Core.ApiResponse.Exceptions
{
    /// <summary>Exception which will lead to 'Internal Server Error' (500) HTTP response</summary>
    public class InternalServerErrorException : BaseException
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Message, which is added to HTTP response (required)</param>
        /// <param name="code">Code, which is added to HTTP response (required)</param>
        /// <param name="additionalData">Additional data, which is added to log (optional)</param>
        /// <param name="innerException">Inner exception (optional)</param>
        public InternalServerErrorException(string message, string code = ResponseCode.UNEXPECTED_ERROR, object additionalData = null, Exception innerException = null)
            : base(message, code, additionalData, innerException)
        {
        }

        /// <summary>HTTP status code of response</summary>
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;
    }
}
