using FroggyDefense.Core.Items;

namespace FroggyDefense.Core
{
    /// <summary>
    /// Defines key methods needed for an inventory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInventory
    {
        public int Size { get; }
        public delegate void InventoryDelegate();
        public event InventoryDelegate InventoryChangedEvent;

        /// <summary>
        /// Adds a set amount of new items to the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int Add(Item item, int amount);

        /// <summary>
        /// Subtracts the item with the given id from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(int itemId, int amount);

        /// <summary>
        /// Removes the item with the given id from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Remove(int itemId);

        /// <summary>
        /// Checks if an item with the input id is in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Contains(int itemId);

        /// <summary>
        /// Checks if there is a certain amount of a given item with the input id in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ContainsAmount(int itemId, int amount);

        /// <summary>
        /// Gets the count of a specific item with the given id.
        /// There is no version for Items because there should not checking for multiples
        /// of a specific instance.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetCount(int itemId);

        /// <summary>
        /// Returns the contents of the inventory as a string.
        /// </summary>
        /// <returns></returns>
        public string ToString();

        /// <summary>
        /// Prints out the contents of the inventory to the console.
        /// </summary>
        public void Print();
    }
}