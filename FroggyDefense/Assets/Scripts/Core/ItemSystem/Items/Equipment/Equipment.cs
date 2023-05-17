using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public enum EquipmentSlot
    {
        Hat,
        Clothes,
        Boots,
        Ring,
        Trinket,
    }

    public class Equipment : Item
    {
        public EquipmentSlot Slot = 0;

        public int Strength = 0;
        public int Dexterity = 0;
        public int Agility = 0;
        public int Intellect = 0;
        public int Spirit = 0;

        public List<StatValuePair> Stats = new List<StatValuePair>();

        /// <summary>
        /// Creates an empty equipment item.
        /// </summary>
        public Equipment()
        {
            Name = "EQUIPMENT";
            Type = ItemType.Equipment;
            IsStackable = false;
        }

        /// <summary>
        /// Creates an item based off the scriptable object template.
        /// </summary>
        /// <param name="template"></param>
        public Equipment(EquipmentObject template)
        {
            Template = template;
            Name = template.Name;
            Description = template.Description;
            Slot = template.Slot;
            Type = template.Type;
            Icon = template.Icon;
            IsStackable = false;

            foreach (StatValuePair stat in template.Stats)
            {
                // TODO: Maybe need to create a new struct? Not sure if that will be important.
                Stats.Add(stat);
            }
        }

        public override bool Use()
        {
            Debug.Log("This is equipment. Equipping " + Name + ".");
            GameManager.instance.m_Player.Equip(this);
            return true;
        }
    }
}