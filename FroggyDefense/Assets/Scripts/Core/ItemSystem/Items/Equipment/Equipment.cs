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

        //public int Strength = 0;
        //public int Dexterity = 0;
        //public int Agility = 0;
        //public int Intellect = 0;
        //public int Spirit = 0;

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

        /// <summary>
        /// Adds a new stat value to the equipment.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void AddStat(StatType stat, int amount)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].Stat == stat)
                {
                    Stats[i] = new StatValuePair(stat, Stats[i].Value + amount);
                    return;
                }
            }
            Stats.Add(new StatValuePair(stat, amount));
        }

        /// <summary>
        /// Removes a stat value from the equipment.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void RemoveStat(StatType stat)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].Stat == stat)
                {
                    Stats.Remove(Stats[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// Checks if the equipment has a particular stat.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public bool ContainsStat(StatType stat)
        {
            foreach (StatValuePair statPair in Stats)
            {
                if (statPair.Stat == stat)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the equipment has a particular stat and gets its value.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetStat(StatType stat)
        {
            foreach (StatValuePair statPair in Stats)
            {
                if (statPair.Stat == stat)
                {
                    return statPair.Value;
                }
            }
            return 0;
        }
    }
}