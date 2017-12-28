namespace MockServer.Net.Client.RunConfiguration
{
    using System.Diagnostics;
    using Entities;

    public abstract class BaseConfiguration
    {
        protected BaseConfiguration(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel)
        {
            this.ServerPort = serverPort;
            this.ProxyPort = proxyPort;
            this.ProxyRemotePort = proxyRemotePort;
            this.ProxyRemoteHost = proxyRemoteHost;
            this.LogLevel = logLevel;
        }

        public int? ServerPort { get; set; }

        public int? ProxyPort { get; set; }

        public string ProxyRemoteHost { get; set; }

        public int? ProxyRemotePort { get; set; }

        public LogLevelEnum? LogLevel { get; set; }

        public abstract string FileName { get; }

        public abstract string BuildCommandLineArguments();

        public ProcessStartInfo BuildStartInfo()
        {
            return new ProcessStartInfo
            {
#if DEBUG
                WindowStyle = ProcessWindowStyle.Normal,
#else
                WindowStyle = ProcessWindowStyle.Hidden,
#endif
                FileName = this.FileName,
                Arguments = this.BuildCommandLineArguments()
            };
        }
    }
}