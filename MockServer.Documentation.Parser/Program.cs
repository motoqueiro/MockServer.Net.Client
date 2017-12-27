namespace MockServer.Documentation.Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var sampleCategories = ParseSamples()
                .GetAwaiter()
                .GetResult();
            StoreSampleCatogories(sampleCategories)
                .GetAwaiter()
                .GetResult();
        }

        private static async Task<ICollection<SampleCategory>> ParseSamples()
        {
            var dp = new Parser();
            return await dp.Parse(
                "http://www.mock-server.com/mock_server/creating_expectations.html",
                "http://www.mock-server.com/mock_server/verification.html",
                "http://www.mock-server.com/mock_server/clearing_and_resetting.html",
                "http://www.mock-server.com/mock_server/mockserver_clients.html",
                "http://www.mock-server.com/mock_server/debugging_issues.html",
                "http://www.mock-server.com/proxy/getting_started.html",
                "http://www.mock-server.com/proxy/record_and_replay.html",
                "http://www.mock-server.com/proxy/verification.html",
                "http://www.mock-server.com/proxy/proxy_clients.html");
        }

        private static async Task StoreSampleCatogories(ICollection<SampleCategory> sampleCategories)
        {
            var samplesPath = Path.Combine(Directory.GetCurrentDirectory(), "Samples");
            foreach (var sampleCategory in sampleCategories)
            {
                await StoreSample(
                    samplesPath,
                    sampleCategory.Samples);
            }
        }

        private static async Task StoreSample(
            string samplesPath,
            List<Sample> samples)
        {
            foreach (var sample in samples)
            {
                var sampleActionPath = Path.Combine(samplesPath, sample.Action);
                Directory.CreateDirectory(sampleActionPath);
                var samplePath = Path.Combine(sampleActionPath, $"{@sample.Title}.json");
                await File.WriteAllTextAsync(samplePath, sample.Body);
            }
        }
    }
}