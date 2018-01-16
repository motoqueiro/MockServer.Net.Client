namespace MockServer.Net.Client.UnitTests
{
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Docker Configuration")]
    [Category("Unit Tests")]
    public class DockerConfigurationUnitTests
    {
        [Test]
        public void FileName_ShouldBeDocker()
        {
            //Act
            var dockerConfiguration = new DockerConfiguration();

            //Assert
            dockerConfiguration.FileName.Should().Be("docker");
        }

        [TestCase(
            null,
            null,
            null,
            null,
            null,
            null,
            "run -d -P jamesdbloom/mockserver")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            null,
            null,
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver")]
        [TestCase(
            1080,
            null,
            null,
            null,
            null,
            null,
            "run -d -p 1080:1080 jamesdbloom/mockserver")]
        [TestCase(
            null,
            1090,
            null,
            null,
            null,
            null,
            "run -d -p 1090:1090 jamesdbloom/mockserver")]
        [TestCase(
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            null,
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -logLevel INFO -serverPort 1080 -proxyPort 1090")]
        [TestCase(
            null,
            1090,
            80,
            "www.mock-server.com",
            null,
            null,
            "run -d -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com")]
        [TestCase(
            1080,
            1090,
            80,
            "www.mock-server.com",
            LogLevelEnum.INFO,
            "-Dmockserver.enableCORSForAllResponses=true -Dmockserver.sslSubjectAlternativeNameDomains='org.mock-server.com,mock-server.com'",
            "run -d -p 1080:1080 -p 1090:1090 jamesdbloom/mockserver /opt/mockserver/run_mockserver.sh -logLevel INFO -serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com -genericJVMOptions \"-Dmockserver.enableCORSForAllResponses=true -Dmockserver.sslSubjectAlternativeNameDomains='org.mock-server.com,mock-server.com'\"")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string jvmOptions,
            string expectedArguments)
        {
            //Arrange
            var configuration = new DockerConfiguration(
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                jvmOptions);

            //Act
            var arguments = configuration.BuildCommandLineArguments();

            //Assert
            arguments.Should().Be(expectedArguments);
            configuration.ServerPort.Should().Be(serverPort);
            configuration.ProxyPort.Should().Be(proxyPort);
            configuration.ProxyRemotePort.Should().Be(proxyRemotePort);
            configuration.ProxyRemoteHost.Should().Be(proxyRemoteHost);
            configuration.LogLevel.Should().Be(logLevel);
            configuration.GenericJVMOptions.Should().Be(jvmOptions);
        }
    }
}