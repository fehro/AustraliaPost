using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Script.Serialization;
using AustraliaPost.Contracts;
using AustraliaPost.Models;

namespace AustraliaPost
{
    public class Connector : IConnector
    {
        #region Global Variables / Propertes

        private readonly string _authKey;

        private const string AustraliaPostApiSearchUrl = "https://auspost.com.au/api/postcode/search.json?";

        #endregion

        #region Constructor

        public Connector(string authKey)
        {
            _authKey = authKey;
        }

        #endregion

        #region Implemented IConnector Members

        /// <summary>
        /// Perform a search with the provided query and optional state and exclude postboxes flag.
        /// </summary>
        public IEnumerable<Locality> Search(string query, string state = null, bool? excludePostboxes = null)
        {
            //Build the search url.
            var url = BuildSearchUrl(query, state, excludePostboxes);

            //Get the response from the api.
            var response = GetFromUrl(url);

            //Deserialize the response.
            return DeserializeResponse(response);
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Deserialize the response.
        /// </summary>
        protected List<Locality> DeserializeResponse(string response)
        {
            try
            {
                return DeserializeJson<SearchResult>(response).Localities.Locality;
            }
            catch
            {
                //Exception during deserialization. Return an empty list.
                return new List<Locality>();
            }
        }

        /// <summary>
        /// Deserialize the JSON data to an object.
        /// </summary>
        protected T DeserializeJson<T>(string json)
        {
            return new JavaScriptSerializer().Deserialize<T>(json);
        }

        /// <summary>
        /// Get the string response from the provided url with a GET request.
        /// </summary>
        protected string GetFromUrl(string url)
        {
            using (var client = new HttpClient())
            {
                //Create the request message.
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("AUTH-KEY", _authKey);

                //Get the response message.
                var response = client.SendAsync(requestMessage).Result;

                //Return the content.
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        
        /// <summary>
        /// Build the search url with the provided values.
        /// </summary>
        protected string BuildSearchUrl(string searchQuery, string state, bool? excludePostboxes)
        {
            var values = new List<KeyValuePair<string, string>>();

            //Build the url key value collection.
            values.Add(new KeyValuePair<string, string>("q", HttpUtility.UrlEncode(searchQuery)));
            values.Add(new KeyValuePair<string, string>("state", state));
            values.Add(new KeyValuePair<string, string>("excludePostBoxFlag", excludePostboxes.ToString().ToLower()));

            return AustraliaPostApiSearchUrl + string.Join("&", values.Select(x => x.Key + "=" + x.Value));
        }
        
        #endregion
    }
}
