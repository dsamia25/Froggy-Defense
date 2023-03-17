using System;
using UnityEngine;

namespace FroggyDefense.Core
{
    // TODO: Make an item ID and a system to automatically assign them for new ItemObjects.
    public class Item : IComparable
    {
        public string Name = "ITEM";
        public string Description = "A NEW ITEM";
        public bool IsStackable { get; set; } = false;

        public Sprite Icon = null;

        public Item ()
        {
            Name = "ITEM";
        }

        public Item(ItemObject template)
        {
            Name = template.Name;
            Description = template.Description;
            IsStackable = template.IsStackable;

            Icon = template.Icon;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}