namespace MockServer.Net.Client.UnitTests
{
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Homebrew Configuration")]
    [Category("Unit Tests")]
    public class HomebrewConfigurationUnitTests
    {
        [Test]
        public void FileName_ShouldBeMockserver()
        {
            //Act
            var homebrewConfiguration = new HomebrewConfiguration();

            //Assert
            homebrewConfiguration.FileName.Should().Be("mockserver");
        }

        [TestCase(
            1080,
            1090,
            null,
            null,
            null,
            "-serverPort 1080 -proxyPort 1090")]
        [TestCase(
            1080,
            null,
            null,
            null,
            null,
            "-serverPort 1080")]
        [TestCase(
            null,
            1090,
            null,
            null,
            null,
            "-proxyPort 1090")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            "-logLevel INFO -serverPort 1080 -proxyPort 1090")]
        [TestCase(
            null,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com")]
        [TestCase(
            1080,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string expectedArguments)
        {
            //Arrange
            var configuration = new HomebrewConfiguration(
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel);

            //Act
            var arguments = configuration.BuildCommandLineArguments();

            //Assert
            arguments.Should().Be(expectedArguments);
            configuration.ServerPort.Should().Be(serverPort);
            configuration.ProxyPort.Should().Be(proxyPort);
            configuration.ProxyRemotePort.Should().Be(proxyRemotePort);
            configuration.ProxyRemoteHost.Should().Be(proxyRemoteHost);
            configuration.LogLevel.Should().Be(logLevel);
        }
    }
}