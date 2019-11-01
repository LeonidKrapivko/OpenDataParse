using BaseCreator.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class Address
    {
        public int Id { get; set; }
        public Area Area { get; set; }
        public Region Region { get; set; }
        public Settlement Settlement { get; set; }
        public Street Street { get; set; }
        public StreetType StreetType { get; set; }
        public int NumberHouse { get; set; }
        public char SymbolHouse { get; set; }
        public int NumberOffice { get; set; }
    } 
}
