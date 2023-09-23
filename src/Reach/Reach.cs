using Reach.Clients;
using Reach.Exceptions;

namespace Reach
{
    /// <summary>
    /// Default Reach Client
    /// </summary>
    public class ReachClient
    {
        private static string _username;
        private static string _password;
        private static IReachRestClient _restClient;
        private static string _logLevel;

        private ReachClient() { }

        /// <summary>
        /// Initialize base client with username and password
        /// </summary>
        /// <param name="username">Auth username</param>
        /// <param name="password">Auth password</param>
        public static void Init(string username, string password)
        {
            SetUsername(username);
            SetPassword(password);
        }

        /// <summary>
        /// Set the client username
        /// </summary>
        /// <param name="username">Auth username</param>
        public static void SetUsername(string username)
        {
            if (username == null)
            {
                throw new AuthenticationException("Username can not be null");
            }

            if (username != _username)
            {
                Invalidate();
            }

            _username = username;
        }

        /// <summary>
        /// Set the client password
        /// </summary>
        /// <param name="password">Auth password</param>
        public static void SetPassword(string password)
        {
            if (password == null)
            {
                throw new AuthenticationException("Password can not be null");
            }

            if (password != _password)
            {
                Invalidate();
            }

            _password = password;
        }


        /// <summary>
        /// Set the logging level
        /// </summary>
        /// <param name="loglevel">log level</param>
        public static void SetLogLevel(string loglevel)
        {
            if (loglevel != _logLevel)
            {
                Invalidate();
            }

            _logLevel = loglevel;
        }

        /// <summary>
        /// Get the rest client
        /// </summary>
        /// <returns>The rest client</returns>
        public static IReachRestClient GetRestClient()
        {
            if (_restClient != null)
            {
                return _restClient;
            }

            if (_username == null || _password == null)
            {
                throw new AuthenticationException(
                    "ReachRestClient was used before ApiUser and ApiKey were set, please call ReachClient.init()"
                );
            }

            _restClient = new ReachRestClient(_username, _password)
            {
                LogLevel = _logLevel
            };
            return _restClient;
        }

        /// <summary>
        /// Set the rest client
        /// </summary>
        /// <param name="restClient">Rest Client to use</param>
        public static void SetRestClient(IReachRestClient restClient)
        {
            _restClient = restClient;
        }

        /// <summary>
        /// Clear out the Rest Client
        /// </summary>
        public static void Invalidate()
        {
            _restClient = null;
        }

    }
}
