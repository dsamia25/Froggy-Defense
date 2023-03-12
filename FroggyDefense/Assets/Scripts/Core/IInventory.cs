namespace FroggyDefense.Core
{
    public interface IInventory
    {
        public bool Add(Item item);

        public bool Add(Item item, int amount);

        public bool Remove(Item item);

        public bool Remove(int index);

        public bool Contains(Item item);

        public Item Get(int index);

        public int GetCount(int index);

        public int GetIndex(Item item);
    }
}