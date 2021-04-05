using RazorEngine;
using RazorEngine.Templating;
using Rosterd.EndToEndTests.Config;

namespace Rosterd.EndToEndTests.Fixtures
{
    public class RazorTemplateEngine
    {
        public string Parse(string filename, object model)
        {
            var template = ResourceFinder.ReadFromResourceFile("Payloads." + filename).Replace("\r\n", "");
            return Engine.Razor.RunCompile(template, filename, null, model); ;
        }
    }
}
