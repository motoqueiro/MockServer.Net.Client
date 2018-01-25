namespace MockServer.Net.Client.IntegrationTests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Integration Tests")]
    public class RestApiClientIntegrationTests
    {
        private MockServerRunner _runner;

        private RestApiClient _client;

        private const int ServerPort = 1080;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var jarPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "mockserver-netty-5.2.3-jar-with-dependencies.jar");
            var configuration = new JavaConfiguration(
                jarPath,
                ServerPort);
            this._runner = new MockServerRunner(configuration);
            this._runner.Start();
            this._client = new RestApiClient(this._runner.RestApiUrl);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this._runner?.Dispose();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Expectation" },
            Category = "Expectation")]
        [Order(1)]
        public async Task CreateExpectation(string jsonData)
        {
            //Act
            var result = await this._client.Expectation(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.Created);
            result.Description.Should().Be("expectation created");
            result.Content.Should().BeEmpty();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Verify" },
            Category = "Verify")]
        [Order(2)]
        public async Task Verify(string jsonData)
        {
            //Act
            var result = await this._client.Verify(jsonData);

            //Assert
            result.Should().NotBeNull();
            if (TestContext.CurrentContext.Test.Name.Contains("Verify Requests Never Received"))
            {
                result.Code.Should().Be(HttpStatusCode.Accepted);
                result.Description.Should().Be("matching request has been received specified number of times");
                result.Content.Should().BeNullOrEmpty();
            }
            else
            {
                result.Code.Should().Be(HttpStatusCode.NotAcceptable);
                result.Description.Should().Be("request has not been received specified numbers of times");
                result.Content.Should().NotBeNullOrEmpty();
            }
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "VerifySequece" },
            Category = "VerifySequence")]
        [Order(3)]
        public async Task VerifySequence(string jsonData)
        {
            //Act
            var result = await this._client.VerifySequence(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.Accepted);
            result.Description.Should().Be("request sequence has been received in specified order");
            result.Content.Should().BeEmpty();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Clear" },
            Category = "Clear")]
        [Order(6)]
        public async Task Clear(
            string jsonData,
            string typeRaw = null)
        {
            //Arrange
            var type = this.ParseNullableEnum<ClearTypeEnum>(typeRaw);

            //Act
            var result = await this._client.Clear(
                jsonData,
                type);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("expectations and recorded requests cleared");
            result.Content.Should().BeEmpty();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Reset" },
            Category = "Reset")]
        [Order(7)]
        public async Task Reset(string typeRaw = null)
        {
            //Arrange
            var type = this.ParseNullableEnum<ObjectTypeEnum>(typeRaw);

            //Act
            var result = await this._client.Reset(type);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("expectations and recorded requests cleared");
            result.Content.Should().BeEmpty();
        }
        
        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Retrieve" },
            Category = "Retrieve")]
        [Order(5)]
        public async Task Retrieve(
            string jsonData,
            string typeRaw = "REQUESTS",
            string formatRaw = "JSON")
        {
            //Arrange
            var type = this.ParseNullableEnum<ObjectTypeEnum>(typeRaw);
            var format = this.ParseNullableEnum<ResponseFormatEnum>(formatRaw);

            //Act
            var result = await this._client.Retrieve(
                jsonData,
                format,
                type);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("recorded requests or active expectations returned");
            result.Content.Should().BeNullOrEmpty();
        }

        [Test]
        [Order(8)]
        public async Task IntegrationTest_Status()
        {
            //Act
            var result = await this._client.Status();

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("MockServer is running and listening on the listed ports");
            result.Content.Should().NotBeNullOrEmpty();
        }

        [TestCaseSource(
            typeof(IntegrationTestCaseData),
            "LoadTestCasesByAction",
            new object[] { "Bind" },
            Category = "Bind")]
        [Order(9)]
        public async Task Bind(string jsonData)
        {
            //Act
            var result = await this._client.Bind(jsonData);

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("listening on additional requested ports, note: the response only contains ports added for the request, to list all ports use /status");
            result.Content.Should().NotBeNull();
        }

        [Test]
        [Order(10)]
        public async Task IntegrationTest_Stop()
        {
            //Act
            var result = await this._client.Stop();

            //Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(HttpStatusCode.OK);
            result.Description.Should().Be("MockServer process is stopping");
            result.Content.Should().BeEmpty();
        }

        private Nullable<T> ParseNullableEnum<T>(string typeRaw) where T : struct
        {
            if (string.IsNullOrEmpty(typeRaw))
            {
                return null;
            }

            return Enum.Parse<T>(typeRaw);
        }
    }
}