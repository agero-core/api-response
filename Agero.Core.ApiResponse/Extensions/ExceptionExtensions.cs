using Agero.Core.ApiResponse.Exceptions;
using Agero.Core.ApiResponse.Models;
using Agero.Core.Checker;
using System;

namespace Agero.Core.ApiResponse.Extensions
{
    /// <summary>Exception extensions</summary>
    public static class ExceptionExtensions
    {
        /// <summary>Converts exception to info response</summary>
        /// <param name="exception">Exception</param>
        /// <param name="code">Response code</param>
        public static InfoResponse ToInfoResponse(this Exception exception, string code = null)
        {
            Check.ArgumentIsNull(exception, "exception");

            return
                new InfoResponse
                (
                    message: exception.Message,
                    code: !string.IsNullOrWhiteSpace(code) ? code : GetCode(exception, ResponseCode.SUCCESS)
                );
        }

        /// <summary>Converts exception to error response</summary>
        /// <param name="exception">Exception</param>
        /// <param name="message">Response message</param>
        /// <param name="code">Response code</param>
        /// <param name="extractAdditionalData">Extracts exception additional data</param>
        public static ErrorResponse ToErrorResponse(this Exception exception, string message = null, string code = null, Func<Exception, object> extractAdditionalData = null)
        {
            Check.ArgumentIsNull(exception, "exception");

            return
                new ErrorResponse
                (
                    message: !string.IsNullOrWhiteSpace(message) ? message : exception.Message,
                    error: exception.ToError(extractAdditionalData),
                    code: !string.IsNullOrWhiteSpace(code) ? code : GetCode(exception, ResponseCode.ERROR)
                );
        }

        /// <summary>Converts exception to error</summary>
        /// <param name="exception">Exception</param>
        /// <param name="extractAdditionalData">Extracts exception additional data</param>
        public static Error ToError(this Exception exception, Func<Exception, object> extractAdditionalData = null)
        {
            Check.ArgumentIsNull(exception, "exception");

            return
                new Error
                (
                    message: exception.Message,
                    type: exception.GetType().ToString(),
                    stackTrace: exception.StackTrace,
                    innerError: exception.InnerException?.ToError(),
                    data: extractAdditionalData != null ? extractAdditionalData(exception) : exception.ExtractAdditionalData()
                );
        }

        /// <summary>Extracts exception additional data</summary>
        /// <param name="exception">Exception</param>
        public static object ExtractAdditionalData(this Exception exception)
        {
            Check.ArgumentIsNull(exception, "exception");

            var baseException = exception as BaseException;

            return baseException?.AdditionalData;
        }

        private static string GetCode(Exception exception, string defaultCode)
        {
            Check.ArgumentIsNull(exception, "exception");
            Check.ArgumentIsNullOrWhiteSpace(defaultCode, "defaultCode");

            var baseException = exception as BaseException;

            return baseException?.Code ?? defaultCode;
        }
    }
}
