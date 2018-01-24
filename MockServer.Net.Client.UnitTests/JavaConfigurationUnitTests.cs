namespace MockServer.Net.Client.UnitTests
{
    using MockServer.Net.Client.Entities;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;

    [TestFixture]
    [Category("Java Configuration")]
    [Category("Unit Tests")]
    public class JavaConfigurationUnitTests
    {
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            null,
            null,
            null,
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080",
            "http://localhost:1080",
            null)]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            80,
            "www.mock-server.com",
            null,
            null,
            null,
            "-jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com",
            "http://localhost:1080",
            "www.mock-server.com:80")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            null,
            null,
            "-Dmockserver.logLevel=INFO -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            LogLevelEnum.WARN,
            null,
            "-Droot.logLevel=WARN -Dmockserver.logLevel=INFO -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
        [TestCase(
            "mockserver-netty-5.3.0-jar-with-dependencies.jar",
            1080,
            1090,
            null,
            null,
            LogLevelEnum.INFO,
            LogLevelEnum.WARN,
            "example_logback.xml",
            "-Droot.logLevel=WARN -Dmockserver.logLevel=INFO -Dlogback.configurationFile=example_logback.xml -jar mockserver-netty-5.3.0-jar-with-dependencies.jar -serverPort 1080 -proxyPort 1090",
            "http://localhost:1080",
            "http://localhost:1090")]
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
            string expectedArguments,
            string expectedRestApiUrl,
            string expectedProxyUrl)
        {
            //Act
            var configuration = new JavaConfiguration(
                jarPath,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                rootLogLevel,
                logLevelConfigurationFilePath);

            //Assert
            AssertHelper.AssertJavaConfiguration(
                configuration,
                jarPath,
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel,
                rootLogLevel,
                logLevelConfigurationFilePath,
                expectedArguments,
                expectedRestApiUrl,
                expectedProxyUrl);
        }
    }
}