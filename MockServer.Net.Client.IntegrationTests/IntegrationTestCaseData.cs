namespace MockServer.Net.Client.IntegrationTests
{
    using System.Collections;
    using System.IO;
    using NUnit.Framework;

    internal class IntegrationTestCaseData
    {
        internal static IEnumerable ExpectationsSource
        {
            get
            {
                return LoadTestCaseData("Expectation");
            }
        }

        private static IEnumerable LoadTestCaseData(string action)
        {
            var jsonDataDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Samples",
                action);
            foreach (var fileName in Directory.GetFiles(jsonDataDirectory, "*.json"))
            {
                var filePath = Path.Combine(
                    jsonDataDirectory,
                    fileName);
                var jsonData = File.ReadAllText(filePath);
                var testName = $"Integration Test - {action} - {Path.GetFileNameWithoutExtension(fileName)}";
                yield return new TestCaseData(jsonData)
                    .SetName(testName)
                    .SetCategory(action);
            }
        }
    }
}