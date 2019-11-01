using BaseCreator.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class AreaFinder : IDisposable
    {
        Int16 id = 1;
        public BinaryTree<Area> areas = new BinaryTree<Area>();
        public Area Add(Area address)
        {
            Area x = new Area {  };
            BinaryTreeNode<Area> res = areas.FindNode(x);
            if (res == null)
            {
                x.Id = id;
                id++;
                areas.Add(x);
                return x;
            }
            else return res.Data;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
