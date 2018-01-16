namespace MockServer.Net.Client.UnitTests
{
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;
    using SimpleFixture;

    [TestFixture]
    [Category("Java Configuration")]
    [Category("Unit Tests")]
    public class JavaConfigurationUnitTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
        }

        [Test]
        public void FileName_ShouldBeJava()
        {
            //Arrange
            var jarPath = this._fixture.Generate<string>();
            var serverPort = this._fixture.Generate<int>();

            //Act
            var javaConfiguration = new JavaConfiguration(
                jarPath,
                serverPort);

            //Assert
            javaConfiguration.FileName.Should().Be("java");
            javaConfiguration.JarPath.Should().Be(jarPath);
            javaConfiguration.ServerPort.Should().Be(serverPort);
        }

        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            null,
            null,
            null,
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            80,
            "www.mock-server.com",
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            null,
            null,
            "-Dmockserver.logLevel=INFO -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            LogLevelEnum.WARN,
            null,
            "-Droot.logLevel=WARN -Dmockserver.logLevel=INFO -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            LogLevelEnum.WARN,
            "example_logback.xml",
            "-Droot.logLevel=WARN -Dmockserver.logLevel=INFO -Dlogback.configurationFile=example_logback.xml -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            string jarPath,
            int serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            LogLevelEnum? rootLogLevel,
            string logLevelConfigurationFilePath,
            string expectedArguments)
        {
            //Arrange
            var configuration = new JavaConfiguration(
                jarPath,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                rootLogLevel,
                logLevelConfigurationFilePath);

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