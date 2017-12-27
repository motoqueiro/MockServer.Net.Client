namespace MockServer.Documentation.Parser
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Dom;

    public class Parser
    {
        private IBrowsingContext _context;

        public Parser()
        {
            var config = Configuration.Default.WithDefaultLoader();
            this._context = BrowsingContext.New(config);
        }

        public async Task<ICollection<SampleCategory>> Parse(params string[] addresses)
        {
            var results = new List<SampleCategory>();
            foreach (var address in addresses)
            {
                var sampleCategories = await  this.Parse(address);
                results.AddRange(sampleCategories);
            }

            return results;
        }

        public async Task<ICollection<SampleCategory>> Parse(string address)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config)
                .OpenAsync(address);
            var titleElements = document.QuerySelectorAll("button.accordion.title");
            if (titleElements == null)
            {
                return null;
            }
            return this.ParseSampleCategories(titleElements)
                .ToList();
        }

        private IEnumerable<SampleCategory> ParseSampleCategories(IHtmlCollection<IElement> titleElements)
        {
            foreach (var titleElement in titleElements)
            {
                var sampleCategory = new SampleCategory
                {
                    Title = titleElement.TextContent,
                    Url = titleElement.BaseUri
                };
                var sampleElements = titleElement.NextElementSibling.QuerySelectorAll("div.panel.title > button.accordion");
                if (sampleElements != null)
                {
                    var samples = this.ParseSamples(sampleElements);
                    sampleCategory.Samples.AddRange(samples);
                }
                yield return sampleCategory;
            }
        }

        private IEnumerable<Sample> ParseSamples(IHtmlCollection<IElement> sampleElements)
        {
            foreach (var sampleElement in sampleElements)
            {
                var restApiButtonElement = sampleElement.NextElementSibling.QuerySelector("button.accordion:contains('REST API')");
                if (restApiButtonElement == null)
                {
                    continue;
                }

                var sample = new Sample
                {
                    Title = sampleElement.TextContent.ToTitleCase()
                };
                var sampleBodyElement = restApiButtonElement.NextElementSibling.QuerySelector("pre > code");
                if (sampleBodyElement != null)
                {
                    sample.Curl = sampleBodyElement.TextContent;
                }
                yield return sample;
            }
        }
    }
}