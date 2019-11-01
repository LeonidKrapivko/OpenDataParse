using System;

namespace BaseCreator
{
    public class Person: IComparable
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public int CompareTo(object obj)
        {
            Person p = obj as Person;
            if (p != null)
                return this.FullName.CompareTo(p.FullName);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}
