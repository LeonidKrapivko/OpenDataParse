using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCreator
{
    public class UniquePersonFinder 
    {
        int id = 1;
        public BinaryTree<Person> peoples = new BinaryTree<Person>();
        public int Add(string founder)
        {
            Person x = new Person { FullName = founder };
            BinaryTreeNode<Person> res = peoples.FindNode(x);
            if (res == null)
            {
                x.Id = id;
                id++;
                peoples.Add(x);
                return x.Id;
            }
            else return res.Data.Id;
        }
    }
}
