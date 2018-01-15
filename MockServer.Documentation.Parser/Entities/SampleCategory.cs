namespace MockServer.Documentation.Parser.Entities
{
    using System.Collections.Generic;

    public class SampleCategory
    {
        public SampleCategory()
        {
            this.Samples = new List<Sample>();
        }

        public string Url { get; set; }

        public string Title { get; set; }

        public List<Sample> Samples { get; set; }
    }
}