﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using IF.Lastfm.Core.Api;

namespace IF.Lastfm.Core
{
    public class LastFm : ILastFm
    {
        #region Constants

        public const string ApiRoot = "http://ws.audioscrobbler.com/2.0/";
        private const string ApiRootFormat = "{0}://ws.audioscrobbler.com/2.0/?method={1}&api_key={2}{3}";

        private const string ResponseFormat = "json";
        
        #endregion

        #region Api objects

        public IAuth Auth { get; set; }

        #endregion

        #region Api helper methods

        internal static string FormatGetApiUrl(string method, string apikey, Dictionary<string, string> parameters = null, bool secure = false)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            parameters.Add("format", ResponseFormat);

            var querystring = FormatQueryParameters(parameters);

            var protocol = secure
                               ? "https"
                               : "http";

            return string.Format(ApiRootFormat, protocol, method, apikey, querystring);
        }

        internal static FormUrlEncodedContent GetPostBody(string method, string apikey, string apisig,
                                                          IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var init = new Dictionary<string, string>
                           {
                               {"method", method},
                               {"api_key", apikey},
                               {"api_sig", apisig},
                               {"format", ResponseFormat}
                           };

            var requestParameters = init.Concat(parameters);

            return new FormUrlEncodedContent(requestParameters);
        }


        private static string FormatQueryParameters(Dictionary<string, string> parameters)
        {
            const string parameterFormat = "&{0}={1}";

            var builder = new StringBuilder();

            foreach (var pair in parameters)
            {
                builder.Append(string.Format(parameterFormat, pair.Key, pair.Value));
            }

            return builder.ToString();
        }

        #endregion
    }
}