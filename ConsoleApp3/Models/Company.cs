using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Edrpou { get; set; }
        public string Address { get; set; }
        public int Boss { get; set; }
        public string Kved { get; set; }
        public string Stan { get; set; }
        public string FoundingDocNum { get; set; }
        public List<int> Founders { get; set; } = new List<int>();
    }
}
