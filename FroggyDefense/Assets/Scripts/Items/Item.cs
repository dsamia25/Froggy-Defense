using System;
using UnityEngine;

namespace FroggyDefense.Core
{
    // TODO: Make more of these.
    public enum ItemType
    {
        Default,
        Equipment
    }

    // TODO: Make an item ID and a system to automatically assign them for new ItemObjects.
    [System.Serializable]
    public class Item : IComparable
    {
        public string Name = "ITEM";
        public string Description = "A NEW ITEM";
        public bool IsStackable { get; set; } = false;
        public ItemType Type = ItemType.Default;
        public int CountSubtractPerUse = 1;

        public Sprite Icon = null;
        public int ShopPrice = 1;

        public Item ()
        {
            Name = "ITEM";
        }

        public Item(ItemObject template)
        {
            Name = template.Name;
            Description = template.Description;
            IsStackable = template.IsStackable;
            Type = template.Type;
            Icon = template.Icon;
            ShopPrice = template.ShopPrice;
        }

        public virtual bool Use()
        {
            Debug.Log("This is an item. Using " + Name + ".");
            return true;
        }

        // TODO: Implement this.
        public int CompareTo(object obj)
        {
            return 1;
        }
    }
}