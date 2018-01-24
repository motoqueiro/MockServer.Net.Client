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
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            1090,
            null,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            null,
            null,
            "-Dmockserver.serverPort=1080 org.mock-server:mockserver-maven-plugin:5.3.0:run",
            "http://localhost:1080",
            null)]
        [TestCase(
            MavenGoalEnum.Run,
            null,
            1090,
            null,
            "-Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run",
            "http://localhost:1090",
            "http://localhost:1090")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            1090,
            LogLevelEnum.INFO,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 -Dmockserver.logLevel=INFO org.mock-server:mockserver-maven-plugin:5.3.0:run",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            MavenGoalEnum.RunForked,
            null,
            1090,
            null,
            "-Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:runForked",
            "http://localhost:1090",
            "http://localhost:1090")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            MavenGoalEnum goal,
            int? serverPort,
            int? proxyPort,
            LogLevelEnum? logLevel,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            //Act
            var configuration = new MavenConfiguration(
                goal,
                serverPort,
                proxyPort,
                logLevel);

            //Assert
            AssertHelper.AssertMavenConfiguration(
                configuration,
                goal,
                serverPort,
                proxyPort,
                logLevel,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }
    }
}