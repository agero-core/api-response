using Agero.Core.Checker;
using System;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Models
{
    /// <summary>HTTP request information</summary>
    [DataContract]
    public class HttpRequestInfo
    {
        /// <summary>Constructor</summary>
        /// <param name="method">HTTP method</param>
        /// <param name="url">URL</param>
        /// <param name="body">HTTP body</param>
        /// <param name="headers">HTTP headers</param>
        public HttpRequestInfo(string method, Uri url, string body, HttpHeaderInfo[] headers)
        {
            Check.ArgumentIsNullOrWhiteSpace(method, "method");
            Check.ArgumentIsNull(url, "url");
            Check.ArgumentIsNull(headers, "headers");

            Method = method;
            Url = url;
            Body = body;
            Headers = headers;
        }

        /// <summary>HTTP method</summary>
        [DataMember(Name = "method")]
        public string Method { get; }

        /// <summary>URL</summary>
        [DataMember(Name = "url")]
        public Uri Url { get; }

        /// <summary>HTTP body</summary>
        [DataMember(Name = "body")]
        public string Body { get; }

        /// <summary>HTTP headers</summary>
        [DataMember(Name = "headers")]
        public HttpHeaderInfo[] Headers { get; }
    }
}
