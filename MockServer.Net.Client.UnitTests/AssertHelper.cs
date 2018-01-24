namespace MockServer.Net.Client.UnitTests
{
    using FluentAssertions;
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;

    internal class AssertHelper
    {
        internal static void AssertJavaConfiguration(
            JavaConfiguration configuration,
            string jarPath,
            int serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            LogLevelEnum? rootLogLevel,
            string logLevelConfigurationFilePath,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            configuration.JarPath.Should().Be(jarPath);
            configuration.RootLogLevel.Should().Be(rootLogLevel);
            configuration.LogLevelConfigurationFilePath.Should().Be(logLevelConfigurationFilePath);
            AssertBaseConfiguration(
                configuration,
                "java",
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }

        internal static void AssertHomebrewConfiguration(
            HomebrewConfiguration configuration,
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            AssertBaseConfiguration(
                configuration,
                "mockserver",
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }

        internal static void AssertDockerConfiguration(
            DockerConfiguration configuration,
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
            configuration.GenericJVMOptions.Should().Be(jvmOptions);
            AssertBaseConfiguration(
                configuration,
                "docker",
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }

        internal static void AssertMavenConfiguration(
            MavenConfiguration configuration,
            MavenGoalEnum goal,
            int? serverPort,
            int? proxyPort,
            LogLevelEnum? logLevel,
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            configuration.Goal.Should().Be(goal);
            AssertBaseConfiguration(
                configuration,
                "mvn",
                serverPort,
                proxyPort,
                logLevel: logLevel,
                expectedArguments: expectedArguments,
                expectedRestApiUrl: expectedRestApiUrl,
                expectedProxyUrl: expectedProxyUrl);
        }

        internal static void AssertBaseConfiguration(
            BaseConfiguration baseConfiguration,
            string fileName,
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null,
            string expectedArguments = null,
            string expectedRestApiUrl = null,
            string expectedProxyUrl = null)
        {
            baseConfiguration.FileName.Should().Be(fileName);
            baseConfiguration.ServerPort.Should().Be(serverPort);
            baseConfiguration.ProxyPort.Should().Be(proxyPort);
            baseConfiguration.ProxyRemotePort.Should().Be(proxyRemotePort);
            baseConfiguration.ProxyRemoteHost.Should().Be(proxyRemoteHost);
            baseConfiguration.LogLevel.Should().Be(logLevel);
            baseConfiguration.BuildCommandLineArguments().Should().Be(expectedArguments);
            baseConfiguration.RestApiUrl.Should().Be(expectedRestApiUrl);
            baseConfiguration.ProxyUrl.Should().Be(expectedProxyUrl);
        }
    }
}