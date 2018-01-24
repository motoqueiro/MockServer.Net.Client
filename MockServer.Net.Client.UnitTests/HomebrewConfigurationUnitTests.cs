namespace MockServer.Net.Client.UnitTests
{
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Homebrew Configuration")]
    [Category("Unit Tests")]
    public class HomebrewConfigurationUnitTests
    {
        [TestCase(
            1080,
            1090,
            null,
            null,
            null,
            "-serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            1080,
            null,
            null,
            null,
            null,
            "-serverPort 1080",
            "http://localhost:1080",
            null)]
        [TestCase(
            null,
            1090,
            null,
            null,
            null,
            "-proxyPort 1090",
            "http://localhost:1090",
            "http://localhost:1090")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            "-logLevel INFO -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            null,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com",
            "http://localhost:1090",
            "www.mock-server.com:80")]
        [TestCase(
            1080,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com",
            "http://localhost:1080",
            "www.mock-server.com:80")]
        public void CommandLineArguments_ShouldBeValid(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            //Act
            var configuration = new HomebrewConfiguration(
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel);

            //Assert
            AssertHelper.AssertHomebrewConfiguration(
                configuration,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }
    }
}