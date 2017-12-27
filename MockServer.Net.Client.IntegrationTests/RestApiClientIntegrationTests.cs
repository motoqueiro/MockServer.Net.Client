namespace MockServer.Net.Client.IntegrationTests
{
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
            var serverUrl = "http://localhost:32775";
            this._client = new RestApiClient(serverUrl);
        }

        [TestCaseSource(typeof(IntegrationTestCaseData), "ExpectationsSource")]
        public async Task CreateExpectation(string jsonData)
        {
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