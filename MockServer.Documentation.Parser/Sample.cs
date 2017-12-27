namespace MockServer.Documentation.Parser
{
    using System;
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

        public NameValueCollection QueryParameters
        {
            get
            {
                return HttpUtility.ParseQueryString(this.Url.Query);
            }
        }

        public string Body
        {
            get
            {
                var index = Array.FindIndex(this.CurlParts, cp => cp == "-d");
                return string.Join(" ", this.CurlParts.Skip(index + 1)).Trim('\'');
            }
        }
    }
}