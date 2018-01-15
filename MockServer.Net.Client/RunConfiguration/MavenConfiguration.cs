namespace MockServer.Net.Client.RunConfiguration
{
    using System.Text;
    using Entities;

    public class MavenConfiguration
        : BaseConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public MavenGoalEnum Goal { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="serverPort"></param>
        /// <param name="proxyPort"></param>
        /// <param name="logLevel"></param>
        public MavenConfiguration(
            MavenGoalEnum goal = MavenGoalEnum.Run,
            int? serverPort = null,
            int? proxyPort = null,
            LogLevelEnum? logLevel = null)
            : base(serverPort, proxyPort, null, null, logLevel)
        {
            this.Goal = goal;
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
                sb.AppendFormat(" -Dmockserver.serverPort={0}", this.ServerPort);
            }

            if (this.ProxyPort.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.proxyPort={0}", this.ProxyPort);
            }

            if (this.LogLevel.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.logLevel={0}", this.LogLevel.ToString());
            }

            var camelCaseGoal = char.ToLowerInvariant(this.Goal.ToString()[0]) + this.Goal.ToString().Substring(1);
            sb.AppendFormat(" org.mock-server:mockserver-maven-plugin:5.3.0:{0}", camelCaseGoal);
            return sb.ToString().Trim();
        }
    }
}