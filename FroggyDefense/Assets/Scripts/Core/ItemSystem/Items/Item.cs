using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public enum ItemType
    {
        Default,
        Equipment,
        Consumable
    }

    public enum ItemRarity
    {
        Trash = 0,
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Mythic = 5,
        Legendary = 6,
        Cool = 7,
        Wicked = 8,
        Sick = 9
    }

    [System.Serializable]
    public class Item
    {
        public ItemObject Template;
        public string Name = "ITEM";
        public int Id { get => (Template != null ? Template.Id : -1); }
        public string Description = "A NEW ITEM";
        public bool IsStackable { get; set; } = false;
        public int StackSize => ItemObject.StackSize;
        public ItemType Type = ItemType.Default;
        public int CountSubtractPerUse = 1;
        public ItemRarity Rarity = 0;

        public Sprite Icon = null;

        public Item ()
        {
            Name = "ITEM";
        }

        public Item(ItemObject template)
        {
            Template = template;
            Name = template.Name;
            Description = template.Description;
            IsStackable = template.IsStackable;
            Type = template.Type;
            Icon = template.Icon;
            Rarity = template.Rarity;
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
            Debug.Log($"Creating {template.Name} as a {item.Type.ToString()} item.");

            return item;
        }

        public virtual bool Use()
        {
            Debug.Log("This is an item. Using " + Name + ".");
            return true;
        }
    }
}