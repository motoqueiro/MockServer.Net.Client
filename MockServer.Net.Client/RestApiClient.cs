﻿namespace MockServer.Net.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Flurl;
    using Flurl.Http;

    public class RestApiClient
    {
        public string Url { get; private set; }

        private static List<Tuple<string, HttpStatusCode, string>> Responses;

        static RestApiClient()
        {
            FlurlHttp.Configure(s => s.AllowedHttpStatusRange = "*");
            Responses = new List<Tuple<string, HttpStatusCode, string>>
            {
                { new Tuple<string, HttpStatusCode, string>("expectation", HttpStatusCode.Created, "expectation created") },
                { new Tuple<string, HttpStatusCode, string>("expectation", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("expectation", HttpStatusCode.NotAcceptable, "invalid expectation") },
                { new Tuple<string, HttpStatusCode, string>("verify", HttpStatusCode.Accepted, "matching request has been received specified number of times") },
                { new Tuple<string, HttpStatusCode, string>("verify", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("verify", HttpStatusCode.NotAcceptable, "request has not been received specified numbers of times") },
                { new Tuple<string, HttpStatusCode, string>("verifySequence", HttpStatusCode.Accepted, "request sequence has been received in specified order") },
                { new Tuple<string, HttpStatusCode, string>("verifySequence", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("verifySequence", HttpStatusCode.NotAcceptable, "request sequence has not been received in specified order") },
                { new Tuple<string, HttpStatusCode, string>("clear", HttpStatusCode.OK, "expectations and recorded requests cleared") },
                { new Tuple<string, HttpStatusCode, string>("clear", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("reset", HttpStatusCode.OK, "expectations and recorded requests cleared") },
                { new Tuple<string, HttpStatusCode, string>("retrieve", HttpStatusCode.OK, "recorded requests or active expectations returned") },
                { new Tuple<string, HttpStatusCode, string>("retrieve", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("status", HttpStatusCode.OK, "MockServer is running and listening on the listed ports") },
                { new Tuple<string, HttpStatusCode, string>("bind", HttpStatusCode.OK, "listening on additional requested ports, note: the response only contains ports added for the request, to list all ports use /status") },
                { new Tuple<string, HttpStatusCode, string>("bind", HttpStatusCode.BadRequest, "incorrect request format") },
                { new Tuple<string, HttpStatusCode, string>("bind", HttpStatusCode.NotAcceptable, "unable to bind to ports (i.e. already bound or JVM process doesn't have permission)") },
                { new Tuple<string, HttpStatusCode, string>("stop", HttpStatusCode.OK, "MockServer process is stopping") }
            };
        }

        public RestApiClient(string serverUrl)
        {
            this.Url = serverUrl;
        }

        /// <summary>
        /// Create expectation.
        /// </summary>
        /// <param name="jsonData">Expectation to create.</param>
        /// <returns></returns>
        public async Task<Response> Expectation(string jsonData)
        {
            return await this.PutRequest("expectation", jsonData);
        }

        /// <summary>
        /// Verify a request has been received a specific number of times.
        /// </summary>
        /// <param name="jsonData">Request matcher and the number of times to match.</param>
        /// <returns></returns>
        public async Task<Response> Verify(string jsonData)
        {
            return await this.PutRequest("verify", jsonData);
        }

        /// <summary>
        /// Verify a sequence of request has been received in the specific order
        /// </summary>
        /// <param name="jsonData">The sequence of requests matchers.</param>
        /// <returns></returns>
        public async Task<Response> VerifySequence(string jsonData)
        {
            return await this.PutRequest("verifysequence", jsonData);
        }

        /// <summary>
        /// clears expectations and recorded requests that match the request matcher.
        /// </summary>
        /// <param name="jsonData">Request used to match expectations and recored requests to clear.</param>
        /// <returns></returns>
        public async Task<Response> Clear(
            string jsonData,
            ClearTypeEnum? type = null)
        {
            return await this.PutRequest("clear", jsonData, new { type });
        }

        /// <summary>
        /// Clears all expectations and recorded requests.
        /// </summary>
        /// <returns></returns>
        public async Task<Response> Reset(ObjectTypeEnum? type = null)
        {
            return await this.PutRequest("reset", null, new { type });
        }

        /// <summary>
        /// Retrieve recorded requests, active expectations, recorded expectations or log messages
        /// </summary>
        /// <param name="jsonData">Request used to match which recorded requests, expectations or log messages to return, an empty body matches all requests, expectations or log messages.</param>
        /// <param name="format">Changes response format, default if not specificed is "json", supported values are "java", "json".</param>
        /// <param name="type">Specifies the type of object that is retrieve, default if not specified is "requests", supported values are "logs", "requests", "recorded_expectations", "active_expectations".</param>
        /// <returns></returns>
        public async Task<Response> Retrieve(
            string jsonData,
            ResponseFormatEnum? format,
            ObjectTypeEnum? type)
        {
            return await this.PutRequest("retrieve", jsonData, new
            {
                format,
                type
            });
        }

        /// <summary>
        /// Return listening ports
        /// </summary>
        /// <returns></returns>
        public async Task<Response> Status()
        {
            return await this.PutRequest("status");
        }

        /// <summary>
        /// Bind additional listening ports.
        /// </summary>
        /// <param>List of ports to bind to, where 0 indicates dynamically bind to any available port.</param>
        /// <returns></returns>
        public async Task<Response> Bind(string jsonData)
        {
            return await this.PutRequest("bind", jsonData);
        }

        /// <summary>
        /// Stop running process.
        /// </summary>
        /// <returns></returns>
        public async Task<Response> Stop()
        {
            return await this.PutRequest("stop");
        }

        private async Task<Response> PutRequest(
            string segment,
            string jsonData = null,
            object queryParameters = null)
        {
            var httpResponse = await this.MakeRequest(
                segment,
                jsonData,
                queryParameters);
            return await this.ResolveResponse(
                segment,
                httpResponse);
        }

        private async Task<HttpResponseMessage> MakeRequest(
            string segment,
            string jsonData,
            object queryParameters)
        {
            var url = this.Url
                .AppendPathSegment(segment);
            if (queryParameters != null)
            {
                url.SetQueryParams(queryParameters, NullValueHandling.Remove);
            }

            var httpResponse = (HttpResponseMessage)null;
            if (!string.IsNullOrEmpty(jsonData))
            {
                httpResponse = await url.PutStringAsync(jsonData);
            }
            else
            {
                httpResponse = await url.PutAsync(null);
            }

            return httpResponse;
        }

        private async Task<Response> ResolveResponse(
            string segment,
            HttpResponseMessage httpResponse)
        {
            var response = Responses.SingleOrDefault(r =>
                string.Compare(r.Item1, segment, StringComparison.InvariantCultureIgnoreCase) == 0
                && r.Item2 == httpResponse.StatusCode);
            if (response == null)
            {
                throw new Exception($"Unexpected MockServer response with code {httpResponse.StatusCode} for {segment}!");
            }

            return new Response
            {
                Code = response.Item2,
                Description = response.Item3,
                Content = await httpResponse.Content.ReadAsStringAsync()
            };
        }
    }
}