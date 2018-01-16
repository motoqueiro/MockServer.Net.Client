namespace MockServer.Net.Client
{
    using System;
    using System.Diagnostics;
    using MockServer.Net.Client.RunConfiguration;

    public class MockServerRunner
        : IDisposable
    {
        private readonly Process _process;

        private readonly BaseConfiguration _configuration;

        public MockServerRunner(BaseConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._process = new Process
            {
                StartInfo = this._configuration.BuildStartInfo()
            };
        }

        public void Start()
        {
            this._process.Start();
            this._process.WaitForExit(50);
            if (this._process.HasExited)
            {
                throw new Exception($"Unable to start mockserver with {this._configuration.ToString()}");
            }
        }

        public string RestApiUrl => this._configuration.RestApiUrl;

        public int ProcessId => this._process.Id;

        public void Kill() => this._process?.Kill();

        public void Dispose() => this.Kill();
    }
}