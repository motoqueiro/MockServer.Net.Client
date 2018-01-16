namespace MockServer.Documentation.Parser
{
    using System.Collections.Generic;
    using System.IO;
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
    }
}