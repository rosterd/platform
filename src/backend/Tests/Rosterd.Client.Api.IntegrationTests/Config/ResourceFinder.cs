using System;
using System.IO;
using System.Reflection;

namespace Rosterd.Client.Api.IntegrationTests.Config
{
    public class ResourceFinder
    {
        public static string ReadFromResourceFile(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosterd.Client.Api.IntegrationTests.Resources." + fileName);
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
