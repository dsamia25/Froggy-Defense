using System;
using UnityEngine;

namespace FroggyDefense.Core.Items
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
            Debug.Log("Item for " + template.Name + " = " + template.ShopPrice + " gems.");
        }

        /// <summary>
        /// Creates the correct type of item.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static Item CreateItem(ItemObject template)
        {
            Item item = null;
            switch (template.Type)
            {
                case ItemType.Default:
                    Debug.Log("Case Default.");
                    item = new Item(template);
                    break;
                case ItemType.Equipment:
                    Debug.Log("Case Equipment.");
                    item = new Equipment((EquipmentObject)template);
                    break;
                default:
                    Debug.Log("Default.");
                    item = new Item(template);
                    break;
            }
            Debug.Log("template Type = " + template.Type + ".");

            return item;
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