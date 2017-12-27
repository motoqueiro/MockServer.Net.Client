namespace Storm.Pricing.Automation.Web.MockServer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using global::MockServer.Net.Client.Entities;
    using global::MockServer.Net.Client.RunConfiguration;

    public class MockServerRunner
    {
        private readonly Process _process;
        private readonly int? _serverPort;
        private readonly int? _proxyPort;
        private readonly int? _proxyRemotePort;
        private readonly string _proxyRemoteHost;
        private readonly LogLevelEnum? _logLevel;

        /// <summary>
        /// Run MockServer from command line using java directly. As documented <see cref="http://www.mock-server.com/mock_server/running_mock_server.html#running_from_command_line_using_java">here</see>.
        /// </summary>
        /// <param name="serverPort">Specifies the HTTP, HTTPS, SOCKS and HTTP CONNECT port for proxy. Port unification supports for all protocols on the same port.</param>
        /// <param name="proxyPort">Specifies the HTTP and HTTPS port for the MockServer.Port unification is used to support HTTP and HTTPS on the same port.</param>
        /// <param name="proxyRemotePort">Specifies the port to forward all proxy requests to(i.e.all requests received on portPort). This setting is used to enable the port forwarding mode therefore this option disables the HTTP, HTTPS, SOCKS and HTTP CONNECT support.</param>
        /// <param name="proxyRemoteHost">Specified the host to forward all proxy requests to(i.e.all requests received on portPort). This setting is ignored unless proxyRemotePort has been specified.If no value is provided for proxyRemoteHost when proxyRemotePort has been specified, proxyRemoteHost will default to "localhost".</param>
        public MockServerRunner(
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null)
        {
            this._serverPort = serverPort;
            this._proxyPort = proxyPort;
            this._proxyRemotePort = proxyRemotePort;
            this._proxyRemoteHost = proxyRemoteHost;
            this._logLevel = logLevel;
            this._process = new Process();
        }

        public void Start(RunTypeEnum runType)
        {
            var configuration = this.BuildConfiguration(runType);
            this._process.StartInfo = configuration.BuildStartInfo();
            this._process.Start();
        }

        public string ResolveUrl()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this._proxyRemoteHost))
            {
                sb.Append(this._proxyRemoteHost);
                if (this._proxyRemotePort.HasValue)
                {
                    sb.AppendFormat(":{0}", this._proxyRemotePort);
                }
            }
            else
            {
                sb.Append("http://localhost");
                if (this._serverPort.HasValue)
                {
                    sb.AppendFormat(":{0}", this._serverPort);
                }
            }

            return sb.ToString();
        }

        public void Stop()
        {
            this._process.Dispose();
        }

        private BaseConfiguration BuildConfiguration(RunTypeEnum runType)
        {
            switch (runType)
            {
                case RunTypeEnum.Docker:
                    return new DockerConfiguration(
                        this._serverPort,
                        this._proxyPort,
                        this._proxyRemotePort,
                        this._proxyRemoteHost,
                        this._logLevel);
                case RunTypeEnum.Maven:
                    return new MavenConfiguration(
                        this._serverPort,
                        this._proxyPort,
                        this._proxyRemotePort,
                        this._proxyRemoteHost,
                        this._logLevel);
                case RunTypeEnum.Java:
                    return new JavaConfiguration(
                        this._serverPort,
                        this._proxyPort,
                        this._proxyRemotePort,
                        this._proxyRemoteHost,
                        this._logLevel);
                case RunTypeEnum.Homebrew:
                    return new HomebrewConfiguration(
                        this._serverPort,
                        this._proxyPort,
                        this._proxyRemotePort,
                        this._proxyRemoteHost,
                        this._logLevel);
                default:
                    throw new SystemException($"Unsupported mockserver run type {runType.ToString()}");
            }
        }
    }
}