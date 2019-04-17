using Agero.Core.Checker;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>Error response</summary>
    [DataContract]
    public class ErrorResponse : InfoResponse
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Response message (required)</param>
        /// <param name="error">Error details (required)</param>
        /// <param name="code">Response code (required)</param>
        public ErrorResponse(string message, Error error, string code = ResponseCode.ERROR) 
            : base(message, code)
        {
            Check.ArgumentIsNull(error, "error");

            Error = error;
        }

        /// <summary>Error details</summary>
        [DataMember(Name = "error")]
        public Error Error { get; }
    }
}
