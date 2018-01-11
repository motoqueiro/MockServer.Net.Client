namespace MockServer.Net.Client.UnitTests
{
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Homebrew Configuration")]
    [Category("Unit Tests")]
    public class MavenConfigurationUnitTests
    {
        [Test]
        public void FileName_ShouldBeMvn()
        {
            //Act
            var mavenConfiguration = new MavenConfiguration();

            //Assert
            mavenConfiguration.FileName.Should().Be("mvn");
        }

        [TestCase(
            1080,
            1090,
            null,
            null,
            null,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090")]
        [TestCase(
            1080,
            null,
            null,
            null,
            null,
            "-Dmockserver.serverPort=1080")]
        [TestCase(
            null,
            1090,
            null,
            null,
            null,
            "-Dmockserver.proxyPort=1090")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 -Dmockserver.logLevel=INFO")]
        [TestCase(
            null,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            1090,
            80,
            "www.mock-server.com",
            null,
            "-serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            MavenGoalEnum goal,
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string expectedArguments)
        {
            //Arrange
            var configuration = new MavenConfiguration(
                goal,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel);

            //Act
            var arguments = configuration.BuildCommandLineArguments();

            //Assert
            arguments.Should().Be(expectedArguments);
            configuration.Goal.Should().Be(goal);
            configuration.ServerPort.Should().Be(serverPort);
            configuration.ProxyPort.Should().Be(proxyPort);
            configuration.ProxyRemotePort.Should().Be(proxyRemotePort);
            configuration.ProxyRemoteHost.Should().Be(proxyRemoteHost);
            configuration.LogLevel.Should().Be(logLevel);
        }
    }
}