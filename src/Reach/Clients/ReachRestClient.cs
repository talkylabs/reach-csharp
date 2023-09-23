
using System;
using System.Net;
using Newtonsoft.Json;
using Reach.Exceptions;

#if !NET35
using System.Threading.Tasks;
#endif

using Reach.Http;
#if NET35
using Reach.Http.Net35;
#endif

namespace Reach.Clients
{
    /// <summary>
    /// Implementation of a ReachRestClient.
    /// </summary>
    public class ReachRestClient : IReachRestClient
    {
        /// <summary>
        /// Client to make HTTP requests
        /// </summary>
        public HttpClient HttpClient { get; }


        /// <summary>
        /// Additions to the user agent string
        /// </summary>
        public string[] UserAgentExtensions { get; set; }

        /// <summary>
        /// Log level for logging
        /// </summary>
        public string LogLevel { get; set; } = Environment.GetEnvironmentVariable("REACH_TALKYLABS_LOG_LEVEL");
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Constructor for a ReachRestClient
        /// </summary>
        ///
        /// <param name="username">username for requests</param>
        /// <param name="password">password for requests</param>
        /// <param name="httpClient">http client used to make the requests</param>
        public ReachRestClient(
            string username,
            string password,
            HttpClient httpClient = null
        )
        {
            _username = username;
            _password = password;
            HttpClient = httpClient ?? DefaultClient();
        }

        /// <summary>
        /// Make a request to the Reach API
        /// </summary>
        ///
        /// <param name="request">request to make</param>
        /// <returns>response of the request</returns>
        public Response Request(Request request)
        {
            request.SetAuth(_username, _password);

            if (LogLevel == "debug")
                LogRequest(request);

            if (UserAgentExtensions != null)
                request.UserAgentExtensions = UserAgentExtensions;

            Response response;
            try
            {
                response = HttpClient.MakeRequest(request);
                if (LogLevel == "debug")
                {
                    Console.WriteLine("response.status: " + response.StatusCode);
                    Console.WriteLine("response.headers: " + response.Headers);
                }
            }
            catch (Exception clientException)
            {
                throw new ApiConnectionException(
                    "Connection Error: " + request.Method + request.ConstructUrl(),
                    clientException
                );
            }
            return ProcessResponse(response);
        }

#if !NET35
        /// <summary>
        /// Make a request to the Reach API
        /// </summary>
        ///
        /// <param name="request">request to make</param>
        /// <returns>Task that resolves to the response of the request</returns>
        public async Task<Response> RequestAsync(Request request)
        {
            request.SetAuth(_username, _password);

            if (UserAgentExtensions != null)
                request.UserAgentExtensions = UserAgentExtensions;

            Response response;
            try
            {
                response = await HttpClient.MakeRequestAsync(request);
            }
            catch (Exception clientException)
            {
                throw new ApiConnectionException(
                    "Connection Error: " + request.Method + request.ConstructUrl(),
                    clientException
                );
            }
            return ProcessResponse(response);
        }

        private static HttpClient DefaultClient()
        {
            return new SystemNetHttpClient();
        }
#else
        private static HttpClient DefaultClient()
        {
            return new WebRequestClient();
        }
#endif

        private static Response ProcessResponse(Response response)
        {
            if (response == null)
            {
                throw new ApiConnectionException("Connection Error: No response received.");
            }

            if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.BadRequest)
            {
                return response;
            }

            // Deserialize and throw exception
            RestException restException = null;
            try
            {
                restException = RestException.FromJson(response.Content);
            }
            catch (JsonReaderException) { /* Allow null check below to handle */ }

            if (restException == null)
            {
                throw new ApiException("Api Error: " + response.StatusCode + " - " + (response.Content ?? "[no content]"));
            }

            throw new ApiException(
                restException.Code,
                (int)response.StatusCode,
                restException.Message ?? "Unable to make request, " + response.StatusCode,
                restException.MoreInfo,
                restException.Details
            );
        }

        
        /// <summary>
        /// Format request information when LogLevel is set to debug
        /// </summary>
        ///
        /// <param name="request">HTTP request</param>
        private static void LogRequest(Request request)
        {
            Console.WriteLine("-- BEGIN Reach API Request --");
            Console.WriteLine("request.method: " + request.Method);
            Console.WriteLine("request.URI: " + request.Uri);

            if (request.QueryParams != null)
            {
                request.QueryParams.ForEach(parameter => Console.WriteLine(parameter.Key + ":" + parameter.Value));
            }

            if (request.HeaderParams != null)
            {
                for (int i = 0; i < request.HeaderParams.Count; i++)
                {
                    var lowercaseHeader = request.HeaderParams[i].Key.ToLower();
                    if (lowercaseHeader.Contains("authorization") == false  && lowercaseHeader.Contains("apikey")==false && lowercaseHeader.Contains("apiuser")==false)
                    {
                        Console.WriteLine(request.HeaderParams[i].Key + ":" + request.HeaderParams[i].Value);
                    }
                }
            }

            Console.WriteLine("-- END Reach API Request --");
        }
    }
}
