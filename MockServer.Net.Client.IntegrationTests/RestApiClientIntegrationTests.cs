namespace MockServer.Net.Client.IntegrationTests
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Integration Tests")]
    public class RestApiClientIntegrationTests
    {
        private MockServerRunner _runner;

        private RestApiClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestContext.WriteLine("Initializing MockServer..");
            var jarPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "mockserver-netty-5.2.3-jar-with-dependencies.jar");
            TestContext.WriteLine($"MockServer jar path: {jarPath}");
            var configuration = new JavaConfiguration(jarPath);
            this._runner = new MockServerRunner(configuration);
            this._runner.Start();
            TestContext.WriteLine($"Mockserver running on process {this._runner.ProcessId}");
            TestContext.WriteLine($"MockServer url: {this._runner.RestApiUrl}");
            this._client = new RestApiClient(this._runner.RestApiUrl);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("Disposing MockServer...");
            this._runner.Dispose();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCaseDataFromJson",
            new object[] { "Expectation" },
            Category = "Expectation")]
        public async Task CreateExpectation(string jsonData)
        {
            //Act
            var result = await this._client.Expectation(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.Created);
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCaseDataFromJson",
            new object[] { "Verify" },
            Category = "Verify")]
        public async Task Verify(string jsonData)
        {
            //Act
            var result = await this._client.Verify(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
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