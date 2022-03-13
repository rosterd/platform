using RazorEngine;
using RazorEngine.Templating;
using Rosterd.Client.Api.IntegrationTests.Config;


namespace Rosterd.Admin.Client.IntegrationTests.Fixtures
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
