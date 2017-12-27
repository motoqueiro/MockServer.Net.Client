namespace Storm.Pricing.Automation.Web.MockServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using global::MockServer.Net.Client;

    public class MockServerRunner
    {
        private Process _process;
        private readonly RestApiClient _client;

        public MockServerRunner(
            int? serverPort = null,
            int? proxyPort = null,
            int? proxyRemotePort = null,
            string proxyRemoteHost = null,
            LogLevelEnum? logLevel = null)
        {
            var arguments = BuildCommandLineArguments(
                serverPort,
                proxyPort,
                proxyRemotePort,
                proxyRemoteHost,
                logLevel);
            Debug.WriteLine(arguments);
            this._process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
#if RELEASE
                    WindowStyle = ProcessWindowStyle.Hidden,
#endif
                    FileName = "java",
                    Arguments = arguments,
                    WorkingDirectory = this.ResolveMockServerPath()
                }
            };

            var mockServerUrl = this.ResolveUrl(
                proxyRemoteHost,
                proxyPort);
            Debug.WriteLine(mockServerUrl);
            this._client = new RestApiClient(mockServerUrl);
        }

        /// <summary>
        /// Run MockServer from command line using java directly. As documented <see cref="http://www.mock-server.com/mock_server/running_mock_server.html#running_from_command_line_using_java">here</see>.
        /// </summary>
        /// <param name="serverPort">Specifies the HTTP, HTTPS, SOCKS and HTTP CONNECT port for proxy. Port unification supports for all protocols on the same port.</param>
        /// <param name="proxyPort">Specifies the HTTP and HTTPS port for the MockServer.Port unification is used to support HTTP and HTTPS on the same port.</param>
        /// <param name="proxyRemotePort">Specifies the port to forward all proxy requests to(i.e.all requests received on portPort). This setting is used to enable the port forwarding mode therefore this option disables the HTTP, HTTPS, SOCKS and HTTP CONNECT support.</param>
        /// <param name="proxyRemoteHost">Specified the host to forward all proxy requests to(i.e.all requests received on portPort). This setting is ignored unless proxyRemotePort has been specified.If no value is provided for proxyRemoteHost when proxyRemotePort has been specified, proxyRemoteHost will default to "localhost".</param>
        public void Start()
        {
            this._process.Start();
        }

        public async Task<IEnumerable<Tuple<string, Response>>> LoadExpectations()
        {
            var path = ResolveMockServerPath();
            var expectationsPath = Path.Combine(path, "Expectations");
            var results = new List<Tuple<string, Response>>();
            foreach (var expectationFile in Directory.EnumerateFiles(expectationsPath, "*.json", SearchOption.AllDirectories))
            {
                var expectationData = File.ReadAllText(expectationFile);
                var response = await this._client.Expectation(expectationData);
                results.Add(new Tuple<string, Response>(expectationFile, response));
            }

            return results;
        }

        public void Stop()
        {
            this._process.Dispose();
        }

        private string BuildCommandLineArguments(
            int? serverPort,
            int? proxyPort,
            int? proxyRemotePort,
            string proxyRemoteHost,
            LogLevelEnum? logLevel)
        {
            var sb = new StringBuilder();
            if (logLevel.HasValue)
            {
                sb.AppendFormat(" -Dmockserver.logLevel={0}", logLevel.ToString());
            }
;
            sb.Append(" -jar mockserver-netty-5.2.3-jar-with-dependencies.jar");
            if (serverPort.HasValue)
            {
                sb.AppendFormat(" -serverPort {0}", serverPort);
            }

            if (proxyPort.HasValue)
            {
                sb.AppendFormat(" -proxyPort {0}", proxyPort);
            }

            if (proxyRemotePort.HasValue)
            {
                sb.AppendFormat(" -proxyRemotePort {0}", proxyRemotePort);
            }

            if (!string.IsNullOrEmpty(proxyRemoteHost))
            {
                sb.AppendFormat(" -proxyRemoteHost {0}", proxyRemoteHost);
            }

            return sb.ToString();
        }

        private string ResolveMockServerPath()
        {
            var directory = Directory.GetCurrentDirectory();
            return Path.Combine(directory, "MockServer");
        }

        private string ResolveUrl(
            string proxyRemoteHost,
            int? serverPort)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(proxyRemoteHost))
            {
                sb.Append("http://localhost");
            }
            else
            {
                sb.Append(proxyRemoteHost);
            }

            if (serverPort.HasValue)
            {
                sb.AppendFormat(":{0}", serverPort);
            }

            return sb.ToString();
        }
    }
}