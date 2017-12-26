namespace MockServer.Net.Client.IntegrationTests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Category("Integration Tests")]
    public class RestApiClientIntegrationTests
    {
        private RestApiClient _client;

        [SetUp]
        public void SetUp()
        {
            var serverUrl = Environment.GetEnvironmentVariable("MOCK_SERVER_URL");
            this._client = new RestApiClient(serverUrl);
        }

        [TestCaseSource(typeof(IntegrationTestCaseData), ""]
        public async Task CreateExpectation(string jsonData)
        {
            //Arrange
            var jsonData = await this.ReadJsonFile("MatchRequestByPath.json");

            //Act
            var result = await this._client.Expectation(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.Created);
        }

        private async Task<string> ReadJsonFile(string fileName)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "JsonData",
                fileName);
            return await File.ReadAllTextAsync(path);
        }
    }
}