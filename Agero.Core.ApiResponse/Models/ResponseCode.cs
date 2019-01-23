namespace Agero.Core.ApiResponse.Models
{
    /// <summary>Response code</summary>
    public static class ResponseCode
    {
        /// <summary>Error response code</summary>
        public const string ERROR = "ERROR";
        
        /// <summary>Success response code</summary>
        public const string SUCCESS = "SUCCESS";

        /// <summary>Unexpected error response code</summary>
        public const string UNEXPECTED_ERROR = "UNEXPECTED_ERROR";

        /// <summary>Invalid request response code</summary>
        public const string INVALID_REQUEST = "INVALID_REQUEST";

        /// <summary>Data not found response code</summary>
        public const string DATA_NOT_FOUND = "DATA_NOT_FOUND";

        /// <summary>Data locked for update response code</summary>
        public const string DATA_LOCKED_FOR_UPDATE = "DATA_LOCKED_FOR_UPDATE";

        /// <summary>Invalid operation response code</summary>
        public const string INVALID_OPERATION = "INVALID_OPERATION";
    }
}
