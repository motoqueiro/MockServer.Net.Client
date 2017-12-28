namespace MockServer.Net.Client.RunConfiguration
{
    using System.Text;
    using Entities;

    public class HomebrewConfiguration
        : BaseConfiguration
    {
        public HomebrewConfiguration(
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null)
            : base(serverPort, proxyPort, proxyRemotePort, proxyRemoteHost, logLevel)
        { }

        public override string FileName
        {
            get
            {
                return "mockserver";
            }
        }

        public override string BuildCommandLineArguments()
        {
            var sb = new StringBuilder();
            if (this.LogLevel.HasValue)
            {
                sb.AppendFormat(" -logLevel {0}", this.LogLevel.ToString());
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