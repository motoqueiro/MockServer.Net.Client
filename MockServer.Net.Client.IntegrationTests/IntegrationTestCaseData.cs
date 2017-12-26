namespace MockServer.Net.Client.IntegrationTests
{
    using System.Collections;
    using System.IO;
    using NUnit.Framework;

    internal class IntegrationTestCaseData
    {
        internal static IEnumerable RequestMatcher
        {
            get
            {
                return LoadTestCaseData("Setup Expectations");
            }
        }

        private static IEnumerable LoadTestCaseData(string action)
        {
            var jsonDataDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "JsonData",
                action);
            foreach (var fileName in Directory.GetFiles(jsonDataDirectory, "*.json"))
            {
                var filePath = Path.Combine(
                    jsonDataDirectory,
                    fileName);
                var jsonData = File.ReadAllTextAsync(filePath);
                yield return new TestCaseData(jsonData)
                    .SetCategory(action);
            }
        }
    }
}