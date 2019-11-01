
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class Relation
    {
        public int RecId { get; set; }
        public int FounderId { get; set; }

        public Relation(int recId, int founderId)
        {
            RecId = recId;
            FounderId = founderId;
        }
    }
}
