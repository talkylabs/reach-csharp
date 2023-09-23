using Newtonsoft.Json;
using System.Collections.Generic;

namespace Reach.Exceptions
{
    /// <summary>
    /// Exception from Reach API
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RestException : ReachException
    {
        /// <summary>
        /// Reach error code
        /// </summary>
        [JsonProperty("errorCode")]
        public int Code { get; private set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        public override string Message {
            get
            {
                return _message;
            }
        }

        [JsonProperty("errorMessage")]
        private string _message
        {
            get; set;
        }

        /// <summary>
        /// More info if provided
        /// </summary>
        [JsonProperty("more_info")]
        public string MoreInfo { get; private set; }

        /// <summary>
        /// Details if provided
        /// </summary>
        [JsonProperty("errorDetails")]
        public Dictionary<string, object> Details { get; private set; }

        /// <summary>
        /// Create an empty RestException
        /// </summary>
        public RestException() {}
        private RestException(
            [JsonProperty("status")]
            int status,
            [JsonProperty("errorMessage")]
            string message,
            [JsonProperty("errorCode")]
            int code,
            [JsonProperty("more_info")]
            string moreInfo,
            [JsonProperty("errorDetails")]
            Dictionary<string, object> details
        ) {
            Status = status;
            Code = code;
            _message = message;
            MoreInfo = moreInfo;
            Details = details;
        }

        /// <summary>
        /// Create a RestException from a JSON payload
        /// </summary>
        /// <param name="json">JSON string to parse</param>
        public static RestException FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RestException>(json);
        }
    }
}
