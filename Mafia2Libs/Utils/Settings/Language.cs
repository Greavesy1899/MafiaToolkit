using System.Collections.Generic;
using System.Xml;
using Utils.Logging;
using Utils.Settings;

namespace Utils.Lang
{
    public class Language
    {
        private static Dictionary<string, string> Localisation = new Dictionary<string, string>();

        public static void ReadLanguageXML()
        {
            Localisation.Clear();
            string xmlToLoad;

            switch (ToolkitSettings.Language)
            {
                case 0:
                    xmlToLoad = "Localisations/en_GB.xml";
                    break;
                case 1:
                    xmlToLoad = "Localisations/ru_RU.xml";
                    break;
                case 2:
                    xmlToLoad = "Localisations/cz_CZ.xml";
                    break;
                case 3:
                    xmlToLoad = "Localisations/pl_PL.xml";
                    break;
                case 4:
                    xmlToLoad = "Localisations/fr_FR.xml";
                    break;
                default:
                    xmlToLoad = "Localisations/en_GB.xml";
                    break;
            }

            Log.WriteLine("Reading Localisation file: " + xmlToLoad);

            XmlDocument document = new XmlDocument();
            document.Load(xmlToLoad);
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
            {
                return text;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Missing {0}", key);
                return key;
            }
        }
    }
}
