namespace MockServer.Documentation.Parser.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;

    public class Sample
    {
        public string Title { get; set; }

        public string Curl { get; set; }


        public string[] CurlParts
        {
            get
            {
                return this.Curl.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public string Method
        {
            get
            {
                var index = Array.FindIndex(this.CurlParts, cp => cp == "-X");
                return this.CurlParts[index + 1];
            }
        }

        public Uri Url
        {
            get
            {
                var index = Array.FindIndex(this.CurlParts, cp => cp == this.Method);
                var url = this.CurlParts[index + 1].Trim('"');
                return new Uri(url);
            }
        }

        public string Action
        {
            get
            {
                return this.Url.Segments[1].ToTitleCase();
            }
        }

        public Dictionary<string, string> QueryParameters
        {
            get
            {
                var queryParameters = HttpUtility.ParseQueryString(this.Url.Query);
                if (queryParameters.Count == 0)
                {
                    return null;
                }

                return queryParameters
                    .AllKeys
                    .ToDictionary(k => k, k => queryParameters[k]);
            }
        }

        public string Body
        {
            get
            {
                var index = Array.FindIndex(this.CurlParts, cp => cp == "-d");
                if (index == -1)
                {
                    return null;
                }
                var body = string.Join(" ", this.CurlParts.Skip(index + 1))
                    .Trim('\'')
                    .Replace("\" + \"", string.Empty);
                return body;
            }
        }
    }
}