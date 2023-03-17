namespace FroggyDefense.Core
{
    /// <summary>
    /// Defines key methods needed for an inventory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInventory
    {
        public delegate void InventoryDelegate();
        public event InventoryDelegate InventoryChangedEvent;

        /// <summary>
        /// Adds a set amount of new items to the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Add(Item item, int amount);

        /// <summary>
        /// Subtracts a given amount of the item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(Item item, int amount);

        /// <summary>
        /// Removes a given item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Item item);

        /// <summary>
        /// Checks if a certain item is in the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Item item);

        /// <summary>
        /// Checks if there is a certain amount of the given item in the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Item item, int amount);

        /// <summary>
        /// Gets the count of a specific item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetCount(Item item);
    }
}