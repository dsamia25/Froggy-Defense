using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public enum BaseItemType
    {
        ItemObject,
        Item
    }

    /// <summary>
    /// BaseItem should be the abstract parent to Item and ItemObject.
    /// Should be able to differentiate which is which using the
    /// BaseItemType enum.
    /// </summary>
    public abstract class BaseItem
    {
        //pub
        //public Sprite Icon { get; protected set; }
        //public string Name { get; protected set; }
        //public int Id { get; }
        //public string Description { get; protected set; }
        //public ItemRarity Rarity { get; protected set; }
        //public ItemType Type { get; protected set; }

        //public bool IsStackable { get; protected set; }
        //public int StackSize { get; }
        //public bool IsUsable { get; protected set; }
        //public int CountSubtractPerUse { get; protected set; }
    }
}