using System;

namespace FroggyDefense.Core
{
    public class Item : IComparable
    {
        public string Name = "ITEM";
        public bool isStackable { get; } = false;
        public int StackSize { get; } = 20;

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}