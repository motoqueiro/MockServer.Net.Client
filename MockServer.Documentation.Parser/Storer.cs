namespace MockServer.Documentation.Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MockServer.Documentation.Parser.Entities;
    using Newtonsoft.Json;

    public class Storer
    {
        public static void Store(
            string filePath,
            IEnumerable<SampleCategory> sampleCategories)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Include;
                serializer.Formatting = Formatting.Indented;
                serializer.ContractResolver = new WritablePropertiesContractResolver();
                serializer.Serialize(file, sampleCategories);
            }
        }

        public static IEnumerable<SampleCategory> Read(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (IEnumerable<SampleCategory>)serializer.Deserialize(file, typeof(IEnumerable<SampleCategory>));
            }
        }

        internal static void GenerateTestCases(
            string testCaseFile,
            ICollection<SampleCategory> sampleCategories)
        {
            var orderedSamples = sampleCategories
                .SelectMany(sc => sc.Samples)
                .OrderBy(s => s.Action);
            using (var sw = new StreamWriter(testCaseFile, false))
            {
                string currentSampleAction = null;
                foreach (var sample in orderedSamples)
                {
                    var arguments = new List<object>();
                    if (!string.IsNullOrEmpty(sample.Body))
                    {
                        arguments.Add("\"" + sample.Body.Replace("\"", "\\\"") + "\"");
                    }
                    if (sample.QueryParameters != null)
                    {
                        arguments.AddRange(sample.QueryParameters.Select(qp => "\"" + qp.Value + "\""));
                    }
                    if (currentSampleAction == null
                        || currentSampleAction != sample.Action)
                    {
                        sw.WriteLine("[Category(\"{0}\")]", sample.Action);
                        currentSampleAction = sample.Action;
                    }
                    sw.WriteLine(
                        "[TestCase({2}, TestName = \"IntegrationTest_{0}_{1}\")]",
                        sample.Action,
                        sample.Title,
                        string.Join(", ", arguments));
                }
            }
        }
    }
}