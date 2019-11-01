using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace BaseCreator
{
    public class DataReader
    {
        Regex rgxCleaner = new Regex("[^a-zA-Z0-9а-я-А-Я,і,І,ї,Ї,.,є,Є -]");
        Regex rgxFounder = new Regex("<FOUNDER>(?<data>.*?)<");
        UniquePersonFinder finder = new UniquePersonFinder();
        int id = 1;


        public string GetArchive()
        {
            string fileArchive = null;
            /// сканирование папки на предмет .zip
            string[] zipArchives = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.zip");
            if (zipArchives.Length == 0)
            {
                Environment.Exit(0);
            }
            fileArchive = zipArchives[0];
            if (zipArchives.Length > 1)
            {
                int num = int.Parse(Console.ReadLine());
                if (num < 0 || num >= zipArchives.Length)
                {
                    Environment.Exit(0);
                }
                else
                {
                    fileArchive = zipArchives[num];
                }
            }
            return fileArchive;
        }

        public IEnumerable<XElement> GetElement(XmlReader reader, string elementName)
        {
            while (!reader.EOF)
                if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
                    yield return XNode.ReadFrom(reader) as XElement;
                else
                    reader.Read();
        }

        public void UnpackDataFromArchive(ZipArchiveEntry file)
        {
            using (XmlReader reader = XmlReader.Create(file.Open()))
            {
                reader.MoveToContent();
                List<Company> records = new List<Company>();
                foreach (var record in GetElement(reader, "RECORD"))
                {
                    if (record == null) continue;
                    records.Add(_recordParse(record));
                }
                List<Person> peoples = finder.peoples.PrintTree();
                DbWorker db = new DbWorker(peoples, records, RelationBuilder(records));
                Console.WriteLine("END");
            }
        }

        private Company _recordParse(XElement element)
        {
            Company company = new Company();
            foreach (var elem in element.Elements())
            {
                switch (elem.Name.ToString())
                {
                    case "NAME": company.Name = rgxCleaner.Replace(elem.Value, ""); break;
                    case "SHORT_NAME": company.ShortName = rgxCleaner.Replace(elem.Value, ""); break;
                    case "EDRPOU": company.Edrpou = Int32.Parse(rgxCleaner.Replace(elem.Value, "")); break;
                    case "ADDRESS": company.Address = rgxCleaner.Replace(elem.Value, ""); break;
                    case "BOSS": company.Boss = finder.Add(rgxCleaner.Replace(elem.Value, "")); break;
                    case "KVED": company.Kved = rgxCleaner.Replace(elem.Value, ""); break;
                    case "STAN": company.Stan = rgxCleaner.Replace(elem.Value, ""); break;
                    case "FOUNDERS":
                        MatchCollection collection = rgxFounder.Matches(elem.ToString());
                        foreach (Match match1 in collection)
                        {
                            string person = _cleanDataToPerson(rgxCleaner.Replace(match1.Groups["data"].Value, ""));
                            if (person != "") company.Founders.Add(finder.Add(person));
                        }
                        break;
                }
            }
            company.Id = id++;
            return company;
        }

        private List<Relation> RelationBuilder(List<Company> companies)
        {
            List<Relation> res = new List<Relation>();
            foreach (var record in companies)
            {
                foreach (var founder in record.Founders)
                {
                    res.Add(new Relation(record.Id, founder));
                }
            }
            return res;
        }

        private string _cleanDataToPerson(string founder)
        {
            Regex rgxFounderBenificiar = new Regex(@"БЕНЕФІЦІАРНИЙ ВЛАСНИК\s?
                (\(?КО?НТРОЛЕР\)?)?\s?-\s?(УКРАЇНА)?\s?(?<name>.*?),
                (\s?\d{5})?(\s?(УКРАЇНА)?)");
            Regex rgxFounder = new Regex("(?<name>.*?), розмір");
            Regex regex = new Regex("(?<name>.*?),.*?$");
            if (founder.Contains("ВІДСУТНІ"))
            {
                return "";
            }
            else if (rgxFounderBenificiar.IsMatch(founder))
            {
                return rgxFounderBenificiar.Match(founder).Groups["name"].Value;
            }
            else if (rgxFounder.IsMatch(founder))
            {
                return rgxFounder.Match(founder).Groups["name"].Value;
            }
            else if (regex.IsMatch(founder))
            {
                return regex.Match(founder).Groups["name"].Value;
            }
            else
            {
                if (founder.Contains("КІНЦЕВИЙ БЕНІФІЦІАР"))
                {
                    return founder.Replace("ЗАСНОВНИК ЮРИДИЧНОЇ ОСОБИ Є КІНЦЕВИЙ БЕНІФІЦІАРНИЙ ВЛАСНИК КОНТРОЛЕР", "");
                };
                return founder;
            }
        }
    }
}
