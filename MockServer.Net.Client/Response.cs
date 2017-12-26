namespace MockServer.Net.Client
{
    using System.Net;

    public class Response
    {
        public HttpStatusCode Code { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
    }
}