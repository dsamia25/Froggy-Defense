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

        public Sprite Icon { get; set; } = null;
        public string Name { get; set; } = "New Item";
        public int Id { get => (Template == null ? -1 : Template.Id); }
        public string Description { get; set; }
        public ItemRarity Rarity { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.Default;

        public bool IsStackable { get; set; } = false;
        public int StackSize { get => ItemObject.StackSize; }
        public int CountSubtractPerUse { get; set; } = 1;
        public bool IsUsable { get; set; } = false;

        public Item ()
        {
            Name = "DEFAULT ITEM";
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
            IsUsable = template.IsUsable;
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
                    item = new Item(template);
                    break;
                case ItemType.Equipment:
                    item = new Equipment((EquipmentObject)template);
                    break;
                case ItemType.Consumable:
                    item = new Consumable((ConsumableObject)template);
                    break;
                default:
                    item = new Item(template);
                    break;
            }
            Debug.Log($"Creating {template.Name} as a {item.Type.ToString()} item.");

            return item;
        }

        /// <summary>
        /// Attempts to use the item.
        /// </summary>
        /// <returns></returns>
        public virtual bool Use()
        {
            if (!IsUsable)
            {
                Debug.Log($"{Name} is not usable.");
                return false;
            }
            Debug.Log($"Using {Name}.");
            return true;
        }

        /// <summary>
        /// Returns the detail text for the item. Should be stats for equipment and
        /// effect for consumables.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDetailText()
        {
            return "";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Item otherItem = obj as Item;
            if (IsStackable && otherItem.IsStackable)
            {
                bool result = Id == otherItem.Id;
                return result;
            }

            return this == otherItem;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}