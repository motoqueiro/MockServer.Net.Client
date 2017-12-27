namespace MockServer.Net.Client.Entities
{
    using System.Net;

    public class Response
    {
        public HttpStatusCode Code { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
    }
}