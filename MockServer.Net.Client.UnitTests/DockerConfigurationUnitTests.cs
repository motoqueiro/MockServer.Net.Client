namespace MockServer.Net.Client.UnitTests
{
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Docker Configuration")]
    [Category("Unit Tests")]
    public class DockerConfigurationUnitTests
    {
        [TestCase(
            null,
            null,
            null,
            null,
            null,
            null,
            "run -d -P jamesdbloom/mockserver",
            null,
            null)]
        [TestCase(
            1080,
            1090,
            null,
            null,
            null,
            null,
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            1080,
            null,
            null,
            null,
            null,
            null,
            "run -d -p 1080:1080 jamesdbloom/mockserver",
            "http://localhost:1080",
            null)]
        [TestCase(
            null,
            1090,
            null,
            null,
            null,
            null,
            "run -d -p 1090:1090 jamesdbloom/mockserver",
            "http://localhost:1090",
            "http://localhost:1090")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            null,
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -logLevel INFO -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            null,
            1090,
            80,
            "www.mock-server.com",
            null,
            null,
            "run -d -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com",
            "http://localhost:1090",
            "www.mock-server.com:80")]
        [TestCase(
            1080,
            1090,
            80,
            "www.mock-server.com",
            LogLevelEnum.INFO,
            "-Dmockserver.enableCORSForAllResponses=true -Dmockserver.sslSubjectAlternativeNameDomains='org.mock-server.com,mock-server.com'",
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -logLevel INFO -serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com -genericJVMOptions \"-Dmockserver.enableCORSForAllResponses=true -Dmockserver.sslSubjectAlternativeNameDomains='org.mock-server.com,mock-server.com'\"",
            "http://localhost:1080",
            "www.mock-server.com:80")]
        public void CommandLineArguments_ShouldBeValid(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string jvmOptions,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            //Act
            var configuration = new DockerConfiguration(
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                jvmOptions);

            //Assert
            AssertHelper.AssertDockerConfiguration(
                configuration,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                jvmOptions,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }
    }
}