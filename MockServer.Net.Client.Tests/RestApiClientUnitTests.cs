namespace MockServer.Net.Client.Tests
{
    using System;
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

        [TestCaseSource(typeof(UnitTestCaseData), "SingleArgumentOkTestData")]
        public async Task GenericClientAction_WithJsonData_ShouldBeOk(
            string methodName,
            HttpStatusCode status,
            string description,
            string body)
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            this._httpTest.RespondWith(body, (int)status);

            //Act
            var response = await Invoke(methodName, jsonData);

            //Assert
            AssertResponse(response, status, description, body);
            this._httpTest.ShouldHaveCalled(this._serverUrl + methodName.ToLowerInvariant())
                .WithVerb(HttpMethod.Put)
                .WithRequestBody(jsonData);
        }

        [TestCaseSource(typeof(UnitTestCaseData), "OkTestData")]
        public async Task GenericClientAction_WithoutJsonData_ShouldBeOk(
            string methodName,
            HttpStatusCode status,
            string description,
            string body)
        {
            //Arrange
            this._httpTest.RespondWith(body, (int)status);

            //Act
            var response = await Invoke(methodName, null);

            //Assert
            AssertResponse(response, status, description, body);
            this._httpTest.ShouldHaveCalled(this._serverUrl + methodName.ToLowerInvariant())
                .WithVerb(HttpMethod.Put);
        }

        [TestCaseSource(typeof(UnitTestCaseData), "ExceptionTestData")]
        public void GenericClientAction_WithJsonData_ShouldThrowException(
            string methodName)
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            this._httpTest.RespondWith(string.Empty, (int)HttpStatusCode.Ambiguous);

            //Act
            Func<Task> func = async () => await Invoke(methodName);

            //Assert
            var exception = func.ShouldThrow<Exception>()
                .WithMessage($"Unexpected MockServer response with code {HttpStatusCode.Ambiguous} for {methodName.ToLower()}!");
        }

        [TestCaseSource(typeof(UnitTestCaseData), "SingleArgumentExceptionTestData")]
        public void GenericClientAction_WithoutJsonData_ShouldThrowException(
            string methodName)
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            this._httpTest.RespondWith(string.Empty, (int)HttpStatusCode.Ambiguous);

            //Act
            Func<Task> func = async () => await Invoke(methodName, new[] { jsonData });

            //Assert
            var exception = func.ShouldThrow<Exception>()
                .WithMessage($"Unexpected MockServer response with code {HttpStatusCode.Ambiguous} for {methodName.ToLower()}!");
        }

        [TearDown]
        public void TearDown()
        {
            this._httpTest.Dispose();
        }

        private async Task<Response> Invoke(string methodName, params object[] parameters)
        {
            var type = this._client.GetType();
            var methodInfo = type.GetMethod(methodName);
            return await (Task<Response>)methodInfo.Invoke(this._client, parameters);
        }

        private void AssertResponse(Response response, HttpStatusCode status, string description, string body)
        {
            response.Should().NotBeNull();
            response.Code.Should().Equals(status);
            response.Description.Should().Equals(description);
            response.Content.Should().Equals(body);
        }
    }
}