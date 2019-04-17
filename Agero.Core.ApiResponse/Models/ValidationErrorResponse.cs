using Agero.Core.Checker;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>Validation error response</summary>
    [DataContract]
    public class ValidationErrorResponse : InfoResponse
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Response message (required)</param>
        /// <param name="errors">Validation errors (required)</param>
        /// <param name="code">Response code (required)</param>
        public ValidationErrorResponse(string message, string[] errors, string code = ResponseCode.INVALID_REQUEST) 
            : base(message, code)
        {
            Check.ArgumentIsNull(errors, "errors");
            Check.Argument(errors.Length > 0, "errors.Length > 0");

            Errors = errors;
        }

        /// <summary>Validation errors</summary>
        [DataMember(Name = "errors")]
        public string[] Errors { get; }
    }
}
