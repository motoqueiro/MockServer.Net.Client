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
            MavenGoalEnum.Run,
            1080,
            1090,
            null,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            null,
            null,
            "-Dmockserver.serverPort=1080 org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [TestCase(
            MavenGoalEnum.Run,
            null,
            1090,
            null,
            "-Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            1090,
            LogLevelEnum.INFO,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 -Dmockserver.logLevel=INFO org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [TestCase(
            MavenGoalEnum.Run,
            null,
            1090,
            null,
            "-Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [TestCase(
            MavenGoalEnum.Run,
            1080,
            1090,
            null,
            "-Dmockserver.serverPort=1080 -Dmockserver.proxyPort=1090 org.mock-server:mockserver-maven-plugin:5.3.0:run")]
        [Category("Command Line Arguments")]
        public void CommandLineArguments_ShouldBeValid(
            MavenGoalEnum goal,
            int? serverPort,
            int? proxyPort,
            LogLevelEnum? logLevel,
            string expectedArguments)
        {
            //Arrange
            var configuration = new MavenConfiguration(
                goal,
                serverPort,
                proxyPort,
                logLevel);

            //Act
            var arguments = configuration.BuildCommandLineArguments();

            //Assert
            arguments.Should().Be(expectedArguments);
            configuration.Goal.Should().Be(goal);
            configuration.ServerPort.Should().Be(serverPort);
            configuration.ProxyPort.Should().Be(proxyPort);
            configuration.LogLevel.Should().Be(logLevel);
        }
    }
}