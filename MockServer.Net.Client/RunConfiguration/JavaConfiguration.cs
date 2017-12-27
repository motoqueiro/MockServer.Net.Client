namespace MockServer.Net.Client.RunConfiguration
{
    using System.Text;
    using Entities;

    public class JavaConfiguration
        : BaseConfiguration
    {
        public string JarPath { get; set; }

        public LogLevelEnum? RootLogLevel { get; set; }

        public string LogLevelConfigurationFilePath { get; set; }

        public JavaConfiguration(
            string jarPath,
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel,
            LogLevelEnum? rootLogLevel,
            string logLevelConfigurationFilePath)
            : base(serverPort, proxyPort, proxyRemotePort, proxyRemoteHost, logLevel)
        {
            this.JarPath = jarPath;
            this.RootLogLevel = rootLogLevel;
            this.LogLevelConfigurationFilePath = logLevelConfigurationFilePath;
        }
        public override string ResolveFileName()
        {
            return "java";
        }

        public override string BuildCommandLineArguments()
        {
            var sb = new StringBuilder();
            if (this.RootLogLevel.HasValue)
            {
                sb.AppendFormat(" -Droot.logLevel={0}", this.RootLogLevel.ToString());
            }

            if (this.LogLevel.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.logLevel={0}", this.LogLevel.Value.ToString());
            }

            if (string.IsNullOrEmpty(this.LogLevelConfigurationFilePath))
            {
                sb.AppendFormat(" -Dlogback.configurationFile={0}", this.LogLevelConfigurationFilePath);
            }

            if (string.IsNullOrEmpty(this.JarPath))
            {
                sb.AppendFormat(" -jar {0}", this.JarPath);
            }

            if (this.ServerPort.HasValue)
            {
                sb.AppendFormat(" -serverPort {0}", this.ServerPort);
            }

            if (this.ProxyPort.HasValue)
            {
                sb.AppendFormat(" -proxyPort {0}", this.ProxyPort);
            }

            if (this.ProxyRemotePort.HasValue)
            {
                sb.AppendFormat(" -proxyRemotePort {0}", this.ProxyRemotePort);
            }

            if (!string.IsNullOrEmpty(this.ProxyRemoteHost))
            {
                sb.AppendFormat(" -proxyRemoteHost {0}", this.ProxyRemoteHost);
            }

            return sb.ToString();
        }
    }
}