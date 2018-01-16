namespace MockServer.Net.Client.IntegrationTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MockServer.Documentation.Parser;
    using MockServer.Documentation.Parser.Entities;
    using NUnit.Framework;

    internal class IntegrationTestCaseData
    {
        public static IEnumerable LoadTestCasesByAction(string action)
        {
            var actionSamples = LoadTestCaseDataFromJson()
                .SelectMany(sc => sc.Samples)
                .Where(s => s.Action == action);
            foreach (var sample in actionSamples)
            {
                var arguments = new List<object>();
                if (!string.IsNullOrEmpty(sample.Body))
                {
                    arguments.Add(sample.Body);
                }
                if (sample.QueryParameters != null)
                {
                    arguments.AddRange(sample.QueryParameters.Select(qp => qp.Value));
                }
                
                yield return new TestCaseData(arguments.ToArray())
                    .SetName($"IntegrationTest_{sample.Action}_{sample.Title}")
                    .SetCategory(sample.Action);
            }
        }

        private static IEnumerable<SampleCategory> LoadTestCaseDataFromJson()
        {
            var jsonFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "sample_categories.json");
            return Storer.Read(jsonFilePath);
        }
    }
}