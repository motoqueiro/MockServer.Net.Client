namespace MockServer.Net.Client.RunConfiguration
{
    using System.Diagnostics;
    using System.Text;
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

        public string RestApiUrl
        {
            get
            {
                if (!this.ServerPort.HasValue
                    && !this.ProxyPort.HasValue)
                {
                    return null;
                }

                var sb = new StringBuilder();
                sb.Append("http://localhost:");
                if (this.ServerPort.HasValue)
                {
                    sb.Append(this.ServerPort);
                }
                else
                {
                    sb.Append(this.ProxyPort);
                }

                return sb.ToString();
            }
        }

        public string ProxyUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ProxyRemoteHost)
                    && this.ProxyRemotePort.HasValue)
                {
                    return $"{this.ProxyRemoteHost}:{this.ProxyRemotePort}";
                }

                if (this.ProxyPort.HasValue)
                {
                    return $"http://localhost:{this.ProxyPort.Value}";
                }

                return null;
            }
        }

        public abstract string BuildCommandLineArguments();

        public override string ToString()
        {
            return $"{this.GetType().Name} -> {this.FileName} {this.BuildCommandLineArguments()}";
        }

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