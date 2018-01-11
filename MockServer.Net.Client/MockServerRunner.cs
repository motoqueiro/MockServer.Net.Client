namespace MockServer.Net.Client
{
    using System;
    using System.Diagnostics;
    using MockServer.Net.Client.RunConfiguration;

    public class MockServerRunner
    {
        private readonly Process _process;

        private readonly BaseConfiguration _configuration;

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

        public void Stop()
        {
            this._process.Dispose();
        }
    }
}