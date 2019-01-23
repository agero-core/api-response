using Agero.Core.Checker;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>Error</summary>
    [DataContract]
    public class Error
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Error message (required)</param>
        /// <param name="type">Error type (required)</param>
        /// <param name="stackTrace">Error stack trace (optional)</param>
        /// <param name="innerError">Inner error (optional)</param>
        /// <param name="data">Error data (optional)</param>
        public Error(string message, string type, string stackTrace = null, Error innerError = null, object data =null)
        {
            Check.ArgumentIsNullOrWhiteSpace(message, "message");
            Check.ArgumentIsNullOrWhiteSpace(type, "type");
            
            Message = message;
            Type = type;
            StackTrace = stackTrace;
            InnerError = innerError;
            Data = data;
        }

        /// <summary>Error message</summary>
        [DataMember(Name = "message")]
        public string Message { get; }

        /// <summary>Error type</summary>
        [DataMember(Name = "type")]
        public string Type { get; }

        /// <summary>Error stack trace</summary>
        [DataMember(Name = "stackTrace")]
        public string StackTrace { get; }

        /// <summary>Inner error</summary>
        [DataMember(Name = "innerError")]
        public Error InnerError { get; }

        /// <summary>Error data</summary>
        [DataMember(Name = "data")]
        public object Data { get; }
    }
}
