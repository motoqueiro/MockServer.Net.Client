namespace MockServer.Net.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Flurl.Http.Testing;
    using NUnit.Framework;
    using SimpleFixture;

    [TestFixture]
    [Category("Unit Tests")]
    public class RestApiClientUnitTests
    {
        private Fixture _fixture;
        private HttpTest _httpTest;
        private string _serverUrl;
        private RestApiClient _client;

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
            this._httpTest = new HttpTest();
            this._serverUrl = this._fixture.Generate<Uri>().ToString();
            this._client = new RestApiClient(this._serverUrl);
        }

        [TestCaseSource(typeof(UnitTestCaseData), "OkTestData")]
        public async Task GenericClientAction_ShouldBeOk(
            string methodName,
            HttpStatusCode status,
            string description,
            string body,
            object[] parameters)
        {
            //Arrange
            this._httpTest.RespondWith(body, (int)status);

            //Act
            var response = await Invoke(
                methodName,
                parameters);

            //Assert
            AssertResponse(response, status, description, body);
            var callAssertion = this._httpTest
                .ShouldHaveCalled(this._serverUrl + methodName.ToLowerInvariant())
                .WithVerb(HttpMethod.Put);
            if (parameters != null
                && !string.IsNullOrEmpty((string)parameters[0]))
            {
                callAssertion.WithRequestBody((string)parameters[0]);
            }
        }

        [TestCaseSource(typeof(UnitTestCaseData), "ExceptionTestData")]
        public void GenericClientAction_ShouldThrowException(
            string methodName,
            HttpStatusCode status,
            string description,
            string body,
            object[] parameters)
        {
            //Arrange
            this._httpTest.RespondWith(body, (int)status);

            //Act
            Func<Task> func = async () => await Invoke(
                methodName,
                parameters);

            //Assert
            var exception = func.ShouldThrow<Exception>()
                .WithMessage($"Unexpected MockServer response with code {status} for {methodName.ToLower()}!");
        }

        [TearDown]
        public void TearDown()
        {
            this._httpTest.Dispose();
        }

        private IEnumerable<object> ResolveParameters(
            string jsonData,
            object[] parameters)
        {
            var parametersList = new List<object>();
            if (!string.IsNullOrEmpty(jsonData))
            {
                parametersList.Add(jsonData);
            }

            if (parameters != null)
            {
                parametersList.AddRange(parameters);
            }
            return parametersList;
        }

        private async Task<Response> Invoke(
            string methodName,
            IEnumerable<object> parameters)
        {
            var type = this._client.GetType();
            var methodInfo = type.GetMethod(methodName);
            return await (Task<Response>)methodInfo.Invoke(
                this._client,
                parameters.ToArray());
        }

        private void AssertResponse(
            Response response,
            HttpStatusCode status,
            string description,
            string body)
        {
            response.Should().NotBeNull();
            response.Code.Should().Equals(status);
            response.Description.Should().Equals(description);
            response.Content.Should().Equals(body);
        }
    }
}