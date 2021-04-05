using System;
using System.IO;
using System.Reflection;

namespace Rosterd.EndToEndTests.Config
{
    public class ResourceFinder
    {
        public static string ReadFromResourceFile(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosterd.EndToEndTests.Resources." + fileName);
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
