using Reach.Http;

namespace Reach.Clients
{
    /// <summary>
    /// Interface for a Reach Client
    /// </summary>
    public interface IReachRestClient
    {
      
        /// <summary>
        /// Get the http client that makes requests
        /// </summary>
        HttpClient HttpClient { get; }

        /// <summary>
        /// Make a request to Reach
        /// </summary>
        ///
        /// <param name="request">Request to make</param>
        /// <returns>response of the request</returns>
        Response Request(Request request);

#if !NET35
        /// <summary>
        /// Make a request to Reach
        /// </summary>
        ///
        /// <param name="request">Request to make</param>
        /// <returns>response of the request</returns>
        System.Threading.Tasks.Task<Response> RequestAsync(Request request);
#endif
    }
}
