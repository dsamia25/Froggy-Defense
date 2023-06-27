using System;
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
            Rarity = template.Rarity;

            foreach (StatValuePair stat in template.Stats)
            {
                AddStat(stat.Stat, Mathf.RoundToInt(stat.Value));
            }

            RollBonusStats(template);
        }

        public override bool Use()
        {
            Debug.Log("This is equipment. Equipping " + Name + ".");
            GameManager.instance.m_Player.Equip(this);
            return true;
        }

        /// <summary>
        /// Adds bonus stats to the item using the random value ranges.
        /// </summary>
        /// <param name="template"></param>
        private void RollBonusStats(EquipmentObject template)
        {
            try
            {
                if (!template.AddBonusStats) return;

                string str = "Rolling bonus stats for " + Name + ":\n{\n";

                // Get a maximum amount of bonus stats this will have.
                int index = 0;
                int bonusStatTotal = UnityEngine.Random.Range(template.RandomStatsRange.x, template.RandomStatsRange.y);
                while (bonusStatTotal > 0)
                {
                    var bonusStat = template.RandomBonusStats[index++ % template.RandomBonusStats.Count];
                    int statRoll = bonusStat.Value;

                    if (statRoll > bonusStatTotal)
                    {
                        statRoll = bonusStatTotal;
                    }

                    str += bonusStat.Stat.ToString() + ": " + statRoll + " ,\n";
                    AddStat(bonusStat.Stat, statRoll);

                    bonusStatTotal -= statRoll;

                    // Try to finish by the 10th try or just break.
                    if (index >= 10) break;
                }
                str += "}";
                Debug.Log(str);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error adding bonus stats: {e}");
            }
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