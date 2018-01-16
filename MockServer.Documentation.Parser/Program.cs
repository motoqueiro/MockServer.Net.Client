namespace MockServer.Documentation.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Loading addresses to parse...");
                var addresses = LoadAddresses().ToArray();
                Console.WriteLine("Parsing addresses...");
                var sampleCategories = Parser.Parse(addresses)
                    .GetAwaiter()
                    .GetResult();
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "sample_categories.json");
                Console.WriteLine("Storage file path {0}", filePath);
                Console.WriteLine("Storing parsed samples...");
                Storer.Store(
                    filePath,
                    sampleCategories);
                Console.WriteLine("Parsing complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The following exception occurred: {ex.Message}");
                throw;
            }
            Console.ReadLine();
        }

        private static IEnumerable<string> LoadAddresses()
        {
            yield return "http://www.mock-server.com/mock_server/creating_expectations.html";
            yield return "http://www.mock-server.com/mock_server/verification.html";
            yield return "http://www.mock-server.com/mock_server/clearing_and_resetting.html";
            yield return "http://www.mock-server.com/mock_server/mockserver_clients.html";
            yield return "http://www.mock-server.com/mock_server/debugging_issues.html";
            yield return "http://www.mock-server.com/proxy/getting_started.html";
            yield return "http://www.mock-server.com/proxy/record_and_replay.html";
            yield return "http://www.mock-server.com/proxy/verification.html";
            yield return "http://www.mock-server.com/proxy/proxy_clients.html";
        }
    }
}