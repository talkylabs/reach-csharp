using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reach.Rest;

#if !NET35
using System.Net;
#else
using System.Web;
#endif

namespace Reach.Http
{
    /// <summary>
    /// Reach request object
    /// </summary>
    public class Request
    {

        /// <summary>
        /// HTTP Method
        /// </summary>
        public HttpMethod Method { get; }

        public Uri Uri { get; private set; }

        /// <summary>
        /// Auth username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Auth password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Additions to the user agent string
        /// </summary>
        public string[] UserAgentExtensions { get; set; }

        /// <summary>
        /// Query params
        /// </summary>
        public List<KeyValuePair<string, string>> QueryParams { get; private set; }

        /// <summary>
        /// Post params
        /// </summary>
        public List<KeyValuePair<string, string>> PostParams { get; private set; }

        /// <summary>
        /// Header params
        /// </summary>
        public List<KeyValuePair<string, string>> HeaderParams { get; private set; }

        /// <summary>
        /// Create a new Reach request
        /// </summary>
        /// <param name="method">HTTP Method</param>
        /// <param name="url">Request URL</param>
        public Request(HttpMethod method, string url)
        {
            Method = method;
            Uri = new Uri(url);
            QueryParams = new List<KeyValuePair<string, string>>();
            PostParams = new List<KeyValuePair<string, string>>();
            HeaderParams = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Create a new Reach request
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="domain">Reach subdomain</param>
        /// <param name="uri">Request URI</param>
        /// <param name="queryParams">Query parameters</param>
        /// <param name="postParams">Post data</param>
        /// <param name="headerParams">Custom header data</param>
        public Request(
            HttpMethod method,
            Domain domain,
            string uri,
            List<KeyValuePair<string, string>> queryParams = null,
            List<KeyValuePair<string, string>> postParams = null,
            List<KeyValuePair<string, string>> headerParams = null
        )
        {
            Method = method;
            Uri = new Uri("https://" + domain + ".reach.talkylabs.com" + uri);

            QueryParams = queryParams ?? new List<KeyValuePair<string, string>>();
            PostParams = postParams ?? new List<KeyValuePair<string, string>>();
            HeaderParams = headerParams ?? new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Construct the request URL
        /// </summary>
        /// <returns>Built URL including query parameters</returns>
        public Uri ConstructUrl()
        {
            var uri = buildUri();
            return QueryParams.Count > 0 ?
                new Uri(uri.AbsoluteUri + "?" + EncodeParameters(QueryParams)) :
                new Uri(uri.AbsoluteUri);
        }

        public Uri buildUri()
        {
            return Uri;
        }

        private string joinIgnoreNull(string separator, List<string> items) {
            StringBuilder builder = new StringBuilder();

            foreach (string item in items) {
                if (item != null) {
                    if (builder.Length > 0) {
                        builder.Append(separator);
                    }

                    builder.Append(item);
                }
            }

            return builder.ToString();
        }

        public string GetUrlStringWithoutPaginationInfo() {
        	return GetUrlStringWithoutSpecificParameters(new List<string> {"page", "pageSize"});
        }

        private string GetUrlStringWithoutSpecificParameters(ICollection<string> parameters) {

            	if(parameters ==null || parameters.Count()==0) {
            		return Uri.ToString();
            	}
              var uriBuilder = new UriBuilder(Uri);
            	string query = Uri.Query;
            	if(query == null || query == "" || query.Length==1){
                return Uri.ToString();
              }

              string[] pieces = query.Substring(1).Split('&');
              List<string> queryParams = pieces.ToList();
              foreach (string par in parameters){
            		string prefix = par+"=";
            		int i = 0;
            		while(i<queryParams.Count()) {
            			if(queryParams.ElementAt(i).StartsWith(prefix)) {
            				queryParams.RemoveAt(i);
            			}else {
            				i++;
            			}
            		}
            	}
            	query = queryParams.Count()==0?null:joinIgnoreNull("&", queryParams);
              uriBuilder.Query = query==null ? "" : "?" + query;
              return uriBuilder.Uri.ToString();
        }

        /// <summary>
        /// Set auth for the request
        /// </summary>
        /// <param name="username">Auth username</param>
        /// <param name="password">Auth password</param>
        public void SetAuth(string username, string password)
        {
            Username = username;
            Password = password;
        }

        private static string EncodeParameters(IEnumerable<KeyValuePair<string, string>> data)
        {
            var result = "";
            var first = true;
            foreach (var pair in data)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result += "&";
                }

#if !NET35
                result += WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value);
#else
                result += HttpUtility.UrlEncode(pair.Key) + "=" + HttpUtility.UrlEncode(pair.Value);
#endif
            }

            return result;
        }

        /// <summary>
        /// Encode POST data for transfer
        /// </summary>
        /// <returns>Encoded byte array</returns>
        public byte[] EncodePostParams()
        {
            return Encoding.UTF8.GetBytes(EncodeParameters(PostParams));
        }

        /// <summary>
        /// Add query parameter to request
        /// </summary>
        /// <param name="name">name of parameter</param>
        /// <param name="value">value of parameter</param>
        public void AddQueryParam(string name, string value)
        {
            AddParam(QueryParams, name, value);
        }

        /// <summary>
        /// Add a parameter to the request payload
        /// </summary>
        /// <param name="name">name of parameter</param>
        /// <param name="value">value of parameter</param>
        public void AddPostParam(string name, string value)
        {
            AddParam(PostParams, name, value);
        }

        /// <summary>
        /// Add a header parameter
        /// </summary>
        /// <param name="name">name of parameter</param>
        /// <param name="value">value of parameter</param>
        public void AddHeaderParam(string name, string value)
        {
            AddParam(HeaderParams, name, value);
        }

        private static void AddParam(ICollection<KeyValuePair<string, string>> list, string name, string value)
        {
            list.Add(new KeyValuePair<string, string>(name, value));
        }

        /// <summary>
        /// Compare request
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>true if requests are equal; false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(Request))
            {
                return false;
            }

            var other = (Request)obj;
            return Method.Equals(other.Method) &&
                   buildUri().Equals(other.buildUri()) &&
                   QueryParams.All(other.QueryParams.Contains) &&
                   other.QueryParams.All(QueryParams.Contains) &&
                   PostParams.All(other.PostParams.Contains) &&
                   other.PostParams.All(PostParams.Contains) &&
                   HeaderParams.All(other.HeaderParams.Contains) &&
                   other.HeaderParams.All(HeaderParams.Contains);
        }

        /// <summary>
        /// Generate hash code for request
        /// </summary>
        /// <returns>generated hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Method?.GetHashCode() ?? 0) ^
                       (buildUri()?.GetHashCode() ?? 0) ^
                       (QueryParams?.GetHashCode() ?? 0) ^
                       (PostParams?.GetHashCode() ?? 0) ^
                       (HeaderParams?.GetHashCode() ?? 0);
            }
        }
    }
}
