using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteJsonTool
{
    public class WebElement
    {
        public string name { get; set; }
        public string locator { get; set; }
        public string value { get; set; }

        public static void GetTemplate(List<WebElement> names, string fileName)
        {

            WriteToFile(names, fileName);
        }

        private static void WriteToFile(List<WebElement> data, string fileName)
        {
            string webElementTemplate;
            foreach (var name in data)
            {
                webElementTemplate = String.Format("\tprivate IWebElement {0}(){{{1}\t var {2}Locator = _parser.GetElementByName({3});{4}\t var {5} = Browser.Driver.WebDriver.InspectElement({6}Locator, _test;{7}\t return {8}{9}\t}}{10}",
                name.name.Replace(" ", ""), Environment.NewLine, name.name.ToLower(), name, Environment.NewLine, name.name.Replace(" ", ""), name.name.Replace(" ", ""), Environment.NewLine, name.name.Replace(" ", ""), Environment.NewLine, Environment.NewLine);
                Console.WriteLine(webElementTemplate);
                string path = @"F:\" + fileName + ".txt";
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(webElementTemplate);
                    tw.Close();

                }
                else if (File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(webElementTemplate);
                        tw.Close();
                    }
                }
            }
        }
    }
}
