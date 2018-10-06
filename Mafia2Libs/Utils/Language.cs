using System.Collections.Generic;
using System.Xml;
using Mafia2;

namespace Mafia2Tool
{
    public class Language
    {
        private static Dictionary<string, string> Localisation = new Dictionary<string, string>();

        public static void ReadLanguageXML()
        {
            Log.WriteLine("Reading Localisation EN_GB");

            XmlDocument document = new XmlDocument();
            document.Load("Localisations/en_gb.xml");
            var nav = document.CreateNavigator();
            var nodes = nav.Select("/Localisation/String");
            while (nodes.MoveNext() == true)
            {
                Localisation.Add(nodes.Current.GetAttribute("ID", ""), nodes.Current.Value);
            }

        }

        public static string GetString(string key)
        {
            string text;

            if (Localisation.TryGetValue(key, out text))
                return text;
            else
                return key;
        }
    }
}
