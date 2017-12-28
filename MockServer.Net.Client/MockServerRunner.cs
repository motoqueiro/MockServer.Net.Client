namespace MockServer.Net.Client
{
    using System;
    using System.Diagnostics;
    using MockServer.Net.Client.RunConfiguration;

    public class MockServerRunner
    {
        private readonly Process _process;
        private readonly BaseConfiguration _configuration;

        /// <summary>
        /// Run MockServer from command line using java directly. As documented <see cref="http://www.mock-server.com/mock_server/running_mock_server.html#running_from_command_line_using_java">here</see>.
        /// </summary>
        /// <param name="serverPort">Specifies the HTTP, HTTPS, SOCKS and HTTP CONNECT port for proxy. Port unification supports for all protocols on the same port.</param>
        /// <param name="proxyPort">Specifies the HTTP and HTTPS port for the MockServer.Port unification is used to support HTTP and HTTPS on the same port.</param>
        /// <param name="proxyRemotePort">Specifies the port to forward all proxy requests to(i.e.all requests received on portPort). This setting is used to enable the port forwarding mode therefore this option disables the HTTP, HTTPS, SOCKS and HTTP CONNECT support.</param>
        /// <param name="proxyRemoteHost">Specified the host to forward all proxy requests to(i.e.all requests received on portPort). This setting is ignored unless proxyRemotePort has been specified.If no value is provided for proxyRemoteHost when proxyRemotePort has been specified, proxyRemoteHost will default to "localhost".</param>
        public MockServerRunner(BaseConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._process = new Process();
        }

        public void Start()
        {
            this._process.StartInfo = this._configuration.BuildStartInfo();
            this._process.Start();
        }

        /*public string Url
        {
            get
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
        }*/

        public void Stop()
        {
            this._process.Dispose();
        }
    }
}