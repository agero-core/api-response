using Agero.Core.Checker;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>Information response</summary>
    [DataContract]
    public class InfoResponse
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Response message (required)</param>
        /// <param name="code">Response code (required)</param>
        public InfoResponse(string message, string code = ResponseCode.SUCCESS)
        {
            Check.ArgumentIsNullOrWhiteSpace(message, "message");
            Check.ArgumentIsNullOrWhiteSpace(code, "code");

            Message = message;
            Code = code;
        }

        /// <summary>Response message</summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>Response code</summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
