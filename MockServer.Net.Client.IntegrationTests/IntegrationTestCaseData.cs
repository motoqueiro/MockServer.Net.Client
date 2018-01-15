namespace MockServer.Net.Client.IntegrationTests
{
    using System.Collections;
    using System.IO;
    using System.Linq;
    using MockServer.Documentation.Parser;
    using NUnit.Framework;

    internal class IntegrationTestCaseData
    {
        public static IEnumerable LoadTestCaseDataFromJson(string action)
        {
            var jsonFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "sample_categories.json");
            var sampleCategories = Storer.Read(jsonFilePath);
            var actionSamples = sampleCategories.SelectMany(sc => sc.Samples)
                .Where(s => s.Action == action);
            foreach (var sample in actionSamples)
            {
                var testName = $"Integration Test - {sample.Action} - {sample.Title}";
                yield return new TestCaseData(sample.Body)
                    .SetName(testName)
                    .SetCategory(action);
            }
        }
    }
}