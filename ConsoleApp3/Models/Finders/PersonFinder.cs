using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class PersonFinder :IDisposable
    {
        int id = 1;
        public BinaryTree<Founder> founders = new BinaryTree<Founder>();
        public int Add(string founder)
        {
            Founder x = new Founder { Person = founder };
            BinaryTreeNode<Founder> res = founders.FindNode(x);
            if (res == null)
            {
                x.Id = id;
                id++;
                founders.Add(x);
                return x.Id;
            }
            else return res.Data.Id;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
