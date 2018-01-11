namespace MockServer.Net.Client.RunConfiguration
{
    using System.Text;
    using Entities;

    public class MavenConfiguration
        : BaseConfiguration
    {
        public MavenGoalEnum Goal { get; set; }

        public bool InitializationClass { get; set; }

        public int Timeout { get; set; }

        public bool Skip { get; set; }

        public MavenConfiguration(
            MavenGoalEnum goal = MavenGoalEnum.Run,
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null,
            int timeout = 0,
            bool skip = false,
            bool initializationClass = false)
            : base(serverPort, proxyPort, proxyRemotePort, proxyRemoteHost, logLevel)
        {
            this.Goal = goal;
            this.Timeout = timeout;
            this.Skip = skip;
            this.InitializationClass = initializationClass;
        }

        public override string FileName
        {
            get
            {
                return "mvn";
            }
        }

        public override string BuildCommandLineArguments()
        {
            var sb = new StringBuilder();
            if (this.ServerPort.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.serverPort {0}", this.ServerPort);
            }

            if (this.ProxyPort.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.proxyPort {0}", this.ProxyPort);
            }

            if (this.LogLevel.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.logLevel {0}", this.LogLevel.ToString());
            }

            sb.AppendFormat(" org.mock-server:mockserver-maven-plugin:5.3.0:{0}", this.Goal);
            return sb.ToString().Trim();
        }
    }
}