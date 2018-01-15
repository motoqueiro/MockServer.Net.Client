namespace MockServer.Documentation.Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
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
                serializer.Serialize(file, sampleCategories);
            }
        }

        public static IEnumerable<SampleCategory> Read(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (IEnumerable<SampleCategory>) serializer.Deserialize(file, typeof(IEnumerable<SampleCategory>));
            }
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