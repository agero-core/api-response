using Agero.Core.Checker;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>HTTP header information</summary>
    [DataContract]
    public class HttpHeaderInfo
    {
        /// <summary>Constructor</summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public HttpHeaderInfo(string key, string[] value)
        {
            Check.ArgumentIsNullOrWhiteSpace(key, "key");
            Check.ArgumentIsNull(value, "value");

            Key = key;
            Value = value;
        }

        /// <summary>Key</summary>
        [DataMember(Name = "key")]
        public string Key { get; }

        /// <summary>Value</summary>
        [DataMember(Name = "value")]
        public string[] Value { get; }
    }
}
