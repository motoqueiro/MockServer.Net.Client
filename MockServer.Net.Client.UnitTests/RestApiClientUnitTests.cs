namespace MockServer.Net.Client.UnitTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Flurl.Http.Testing;
    using MockServer.Net.Client.Entities;
    using NUnit.Framework;
    using SimpleFixture;

    [TestFixture]
    [Category("Rest API Client")]
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

        [Category("Expectation")]
        [TestCase(201, "expectation created", TestName = "CreateExpectation_ShouldReturnCreated")]
        [TestCase(400, "incorrect request format", TestName = "CreateExpectation_ShouldReturnBadRequest")]
        [TestCase(406, "invalid expectation", TestName = "CreateExpectation_ShouldReturnNotAcceptable")]
        public async Task CreateExpectationRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);
            var jsonData = this._fixture.Generate<string>();

            //Act
            var response = await this._client.Expectation(jsonData);

            //Assert
            AssertResponse(response, status, description);
        }

        [Category("Reset")]
        [TestCase(null, 200, "expectations and recorded requests cleared", TestName = "ResetRequest_ShouldReturnOk")]
        [TestCase(ObjectTypeEnum.ACTIVE_EXPECTATIONS, 200, "expectations and recorded requests cleared", TestName = "ResetRequest_ActiveExpectationsType_ShouldReturnOk")]
        [TestCase(ObjectTypeEnum.LOGS, 200, "expectations and recorded requests cleared", TestName = "ResetRequest_LogsType_ShouldReturnOk")]
        [TestCase(ObjectTypeEnum.RECORDED_EXPECTATIONS, 200, "expectations and recorded requests cleared", TestName = "ResetRequest_RecordedExpectationsType_ShouldReturnOk")]
        [TestCase(ObjectTypeEnum.REQUESTS, 200, "expectations and recorded requests cleared", TestName = "ResetRequest_RequestsType_ShouldReturnOk")]
        public async Task ResetRequestTest(
            ObjectTypeEnum? type,
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var response = await this._client.Reset(type);

            //Assert
            AssertResponse(response, status, description);
            var callAssertion = this._httpTest
                .ShouldHaveCalled(this._serverUrl + "reset");
            if (type.HasValue)
            {
                callAssertion.WithQueryParamValue("type", type);
            }

            callAssertion.WithVerb(HttpMethod.Put);
        }

        [Category("Retrieve")]
        [TestCase(200, "recorded requests or active expectations returned", ResponseFormatEnum.JSON, ObjectTypeEnum.REQUESTS, TestName = "RetrieveRequest_JsonFormat_RequestsType_ShouldReturnOk")]
        [TestCase(200, "recorded requests or active expectations returned", ResponseFormatEnum.JSON, ObjectTypeEnum.REQUESTS, TestName = "RetrieveRequest_NullFormat_NullType_ShouldReturnOk")]
        [TestCase(400, "incorrect request format", ResponseFormatEnum.JSON, ObjectTypeEnum.REQUESTS, TestName = "RetrieveRequest_JsonFormat_RequestsType_ShouldReturBadRequest")]
        public async Task RetrieveRequestTest(
            HttpStatusCode status,
            string description,
            ResponseFormatEnum? format,
            ObjectTypeEnum? type)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);
            var jsonData = this._fixture.Generate<string>();

            //Act
            var response = await this._client.Retrieve(
                jsonData,
                format,
                type);

            //Assert
            AssertResponse(response, status, description);
            var callAssertion = this._httpTest
                .ShouldHaveCalled(this._serverUrl + "retrieve");
            if (format.HasValue)
            {
                callAssertion.WithQueryParamValue("format", format);
            }
            if (type.HasValue)
            {
                callAssertion.WithQueryParamValue("type", type);
            }
            callAssertion.WithRequestJson(jsonData)
                .WithVerb(HttpMethod.Put);
        }

        [Category("Status")]
        [TestCase(200, "MockServer is running and listening on the listed ports", TestName = "StatusRequest_ShouldReturnOk")]
        public async Task StatusRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var response = await this._client.Status();

            //Assert
            AssertResponse(response, status, description);
            this._httpTest
                .ShouldHaveCalled(this._serverUrl + "status")
                .WithVerb(HttpMethod.Put);
        }

        [Category("Stop")]
        [TestCase(200, "MockServer process is stopping", TestName = "StopRequest_ShouldReturnOk")]
        public async Task StopRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var response = await this._client.Stop();

            //Assert
            AssertResponse(response, status, description);
            this._httpTest
                .ShouldHaveCalled(this._serverUrl + "stop")
                .WithVerb(HttpMethod.Put);
        }

        [Category("Verify")]
        [TestCase(200, "matching request has been received specified number of times", TestName = "VerifyRequest_ShouldReturnOk")]
        [TestCase(400, "incorrect request format", TestName = "VerifyRequest_ShouldReturnBadRequest")]
        [TestCase(406, "request has not been received specified numbers of times", TestName = "VerifyRequest_ShouldReturnNotAcceptable")]
        public async Task VerifyRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);
            var jsonData = this._fixture.Generate<string>();

            //Act
            var response = await this._client.Verify(jsonData);

            //Assert
            AssertResponse(response, status, description, jsonData);
            this._httpTest
                .ShouldHaveCalled(this._serverUrl + "verify")
                .WithRequestJson(jsonData)
                .WithVerb(HttpMethod.Put);
        }

        [Category("VerifySequence")]
        [TestCase(202, "request sequence has been received in specified order", TestName = "VerifySequenceRequest_ShouldReturnAccepted")]
        [TestCase(400, "request has not been received specified numbers of times", TestName = "VerifySequenceRequest_ShouldReturnBadRequest")]
        [TestCase(406, "request has not been received specified numbers of times", TestName = "VerifySequenceRequest_ShouldReturnNotAcceptable")]
        public async Task VerifySequenceRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);
            var jsonData = this._fixture.Generate<string>();

            //Act
            var response = await this._client.VerifySequence(jsonData);

            //Assert
            AssertResponse(response, status, description, jsonData);
            this._httpTest
                .ShouldHaveCalled(this._serverUrl + "verifysequence")
                .WithRequestJson(jsonData)
                .WithVerb(HttpMethod.Put);
        }

        [Category("Clear")]
        [TestCase(null, 200, "expectations and recorded requests cleared", TestName = "ClearRequest_ShouldReturnOk")]
        [TestCase(ClearTypeEnum.EXPECTATIONS, 200, "expectations and recorded requests cleared", TestName = "ClearRequest_ExpectationsType_ShouldReturnOk")]
        [TestCase(ClearTypeEnum.ALL, 200, "expectations and recorded requests cleared", TestName = "ClearRequest_AllType_ShouldReturnOk")]
        [TestCase(ClearTypeEnum.LOG, 200, "expectations and recorded requests cleared", TestName = "ClearRequest_LogType_ShouldReturnOk")]
        [TestCase(null, 400, "incorrect request format", TestName = "ClearRequest_ShouldReturnBadRequest")]
        public async Task ClearRequestTest(
            ClearTypeEnum? type,
            HttpStatusCode status,
            string description)
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var response = await this._client.Clear(
                jsonData,
                type);

            //Assert
            AssertResponse(response, status, description, jsonData);
            var callAssertion = this._httpTest
                .ShouldHaveCalled(this._serverUrl + "clear");
            if (type.HasValue)
            {
                callAssertion = callAssertion.WithQueryParamValue(nameof(type), type);
            }

            callAssertion.WithRequestJson(jsonData)
                .WithVerb(HttpMethod.Put);
        }

        [Category("Bind")]
        [TestCase(200, "listening on additional requested ports, note: the response ony contains ports added for the request, to list all ports use /status", TestName = "BindRequest_ShouldReturnOk")]
        [TestCase(400, "incorrect request format", TestName = "BindRequest_ShouldReturnBadRequest")]
        [TestCase(406, "unable to bind to ports(i.e.already bound or JVM process doesn't have permission)", TestName = "BindRequest_ShouldReturnNotAcceptable")]
        public async Task BindRequestTest(
            HttpStatusCode status,
            string description)
        {
            //Arrange
            this._httpTest.RespondWith(string.Empty, (int)status);
            var jsonData = this._fixture.Generate<string>();

            //Act
            var response = await this._client.Bind(jsonData);

            //Assert
            AssertResponse(response, status, description, jsonData);
            this._httpTest
                .ShouldHaveCalled(this._serverUrl + "bind")
                .WithRequestJson(jsonData)
                .WithVerb(HttpMethod.Put);
        }

        [Category("Reset")]
        [Test]
        public void ResetRequest_ShouldThrowException()
        {
            //Arrange
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Reset());

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for reset!");
        }

        [Category("Retrieve")]
        [Test]
        public void RetrieveRequest_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var format = ResponseFormatEnum.JSON;
            var type = ObjectTypeEnum.REQUESTS;
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Retrieve(
                jsonData,
                format,
                type));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for retrieve!");
        }

        [Category("Status")]
        [Test]
        public void StatusRequest_ShouldThrowException()
        {
            //Arrange
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Status());

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for status!");
        }

        [Category("Stop")]
        [Test]
        public void StopRequest_ShouldThrowException()
        {
            //Arrange
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Stop());

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for stop!");
        }

        [Category("Expectation")]
        [Test]
        public void CreateExpectation_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Expectation(jsonData));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for expectation!");
        }

        [Category("Verify")]
        [Test]
        public void VerifyRequest_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Verify(jsonData));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for verify!");
        }

        [Category("VerifySequence")]
        [Test]
        public void VerifySequenceRequest_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.VerifySequence(jsonData));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for verifysequence!");
        }

        [Category("Clear")]
        [Test]
        public void ClearRequest_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Clear(jsonData));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for clear!");
        }

        [Category("Bind")]
        [Test]
        public void BindRequest_ShouldThrowException()
        {
            //Arrange
            var jsonData = this._fixture.Generate<string>();
            var status = HttpStatusCode.Conflict;
            this._httpTest.RespondWith(string.Empty, (int)status);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await this._client.Bind(jsonData));

            //Assert
            exception.Message.Should().Be($"Unexpected MockServer response with code {status} for bind!");
        }

        [TearDown]
        public void TearDown()
        {
            this._httpTest.Dispose();
        }

        private void AssertResponse(
            Response response,
            HttpStatusCode status,
            string description,
            string body = null)
        {
            response.Should().NotBeNull();
            response.Code.Should().Equals(status);
            response.Description.Should().Equals(description);
            if (!string.IsNullOrEmpty(body))
            {
                response.Content.Should().Equals(body);
            }
        }
    }
}