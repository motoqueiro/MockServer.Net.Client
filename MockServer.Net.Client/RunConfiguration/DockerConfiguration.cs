namespace MockServer.Net.Client.RunConfiguration
{
    using System.Text;
    using Entities;

    public class DockerConfiguration
        : BaseConfiguration
    {
        public string GenericJVMOptions { get; set; }

        public DockerConfiguration(
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null,
            string jvmOptions = null)
            : base(serverPort, proxyPort, proxyRemotePort, proxyRemoteHost, logLevel)
        {
            this.GenericJVMOptions = jvmOptions;
        }

        public override string FileName
        {
            get
            {
                return "docker";
            }
        }

        public override string BuildCommandLineArguments()
        {
            var sb = new StringBuilder("run -d");
            if (!this.ServerPort.HasValue
                && !this.ProxyPort.HasValue)
            {
                sb.Append(" -P");
            }
            else
            {
                if (this.ServerPort.HasValue)
                {
                    sb.AppendFormat(" -p {0}:1080", this.ServerPort);
                }

                if (this.ProxyPort.HasValue)
                {
                    sb.AppendFormat(" -p {0}:1090", this.ProxyPort);
                }
            }

            sb.Append(" jamesdbloom/mockserver");
            if (this.LogLevel.HasValue
                || !string.IsNullOrEmpty(this.ProxyRemoteHost)
                || this.ProxyRemotePort.HasValue
                || !string.IsNullOrEmpty(this.GenericJVMOptions))
            {
                sb.Append(" /opt/mockserver/run_mockserver.sh");
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

                if (!string.IsNullOrEmpty(this.GenericJVMOptions))
                {
                    sb.AppendFormat(" -genericJVMOptions \"{0}\"", this.GenericJVMOptions);
                }
            }

            return sb.ToString();
        }
    }
}