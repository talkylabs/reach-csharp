using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reach.Rest;
using Reach.Exceptions;
using static System.String;

namespace Reach.Base
{
    /// <summary>
    /// Page of resources
    /// </summary>
    /// <typeparam name="T">Resource type</typeparam>
    public class Page<T> where T : Resource
    {
        /// <summary>
        /// Records for this page
        /// </summary>
        public List<T> Records { get; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; }

        private readonly string _url;
        private readonly bool _outOfPageRange;
        private readonly int _totalPages;
        private readonly int _currentPage;

        private Page(
            List<T> records,
            int pageSize,
            string url = null,
            bool outOfPageRange = true,
            int totalPages = 1,
            int currentPage = 0
        )
        {
            Records = records;
            PageSize = pageSize;
            _url = url;
            _outOfPageRange = outOfPageRange;
            _totalPages = totalPages;
            _currentPage = currentPage;
        }

        private string getUrlString(int pageSize, int page)
        {
          string query = "pageSize="+pageSize+"&page="+page;
          try{
            Uri parsedUrl = new Uri(_url);
            string result = (parsedUrl.Query == "" || parsedUrl.Query == null) ? "?": (parsedUrl.Query.Length==1?"":"&");
  			    result = _url + result + query;
  			    return result;
          }catch (Exception){
            throw new ApiException("Malformed Url: " + _url);
          }

        }

        /// <summary>
        /// Generate the first page URL
        /// </summary>
        /// <param name="domain">Reach subdomain</param>
        /// <returns>URL for the first page of results</returns>
        public string GetFirstPageUrl(Domain domain)
        {
            return getUrlString(PageSize, 0);
        }

        /// <summary>
        /// Get the next page URL
        /// </summary>
        /// <param name="domain">Reach subdomain</param>
        /// <returns>URL for the next page of results</returns>
        public string GetNextPageUrl(Domain domain)
        {
          if(!HasNextPage()) {
        		throw new ApiException("No next page available");
        	}
          return getUrlString(PageSize, _currentPage + 1);
        }

        /// <summary>
        /// Get the previous page URL
        /// </summary>
        /// <param name="domain">Reach subdomain</param>
        /// <returns>URL for the previous page of results</returns>
        public string GetPreviousPageUrl(Domain domain)
        {
          if(!HasPreviousPage()) {
            throw new ApiException("No previous page available");
          }
          return getUrlString(PageSize, _currentPage - 1);
        }

        /// <summary>
        /// Get the URL for the current page
        /// </summary>
        /// <param name="domain">Reach subdomain</param>
        /// <returns>URL for the current page of results</returns>
        public string GetUrl(Domain domain)
        {
            return getUrlString(PageSize, _currentPage);
        }

        /// <summary>
        /// Determines if there is another page of results
        /// </summary>
        /// <returns>true if there is a next page; false otherwise</returns>
        public bool HasNextPage()
        {
            return !(_outOfPageRange || (_currentPage + 1 >= _totalPages));
        }

        /// <summary>
        /// Determines if there is another page of results
        /// </summary>
        /// <returns>true if there is a previous page; false otherwise</returns>
        public bool HasPreviousPage()
        {
            return _currentPage > 0;
        }

        /// <summary>
        /// Converts a JSON payload to a Page of results
        /// </summary>
        /// <param name="url">the url to get the page</param>
        /// <param name="recordKey">JSON key where the records are</param>
        /// <param name="json">JSON payload</param>
        /// <returns>Page of results</returns>
        public static Page<T> FromJson(string url, string recordKey, string json)
        {
            var root = JObject.Parse(json);
            var records = root[recordKey];
            var parsedRecords = records.Children().Select(
                record => JsonConvert.DeserializeObject<T>(record.ToString())
            ).ToList();


            JToken pageSize;
            JToken outOfPageRange;
            JToken totalPages;
            JToken currentPage;

            return new Page<T>(
                parsedRecords,
                root.TryGetValue("pageSize", out pageSize) ? root["pageSize"].Value<int>() : parsedRecords.Count,
                url: url, //uriNode.Value<string>(),
                outOfPageRange: root.TryGetValue("outOfPageRange", out outOfPageRange) ? root["outOfPageRange"].Value<bool>() : true,
                totalPages: root.TryGetValue("totalPages", out totalPages) ? root["totalPages"].Value<int>() : 1,
                currentPage: root.TryGetValue("page", out currentPage) ? root["page"].Value<int>() : 0
            );

        }
    }
}
