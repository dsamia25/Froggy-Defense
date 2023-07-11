using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public class Consumable : Item
    {
        public int healthRestore = 0;
        public int manaRestore = 0;
        public float regenTime = 0;

        /// <summary>
        /// Creates an empty equipment item.
        /// </summary>
        public Consumable()
        {
            Name = "Consumable";
            Type = ItemType.Consumable;
            IsStackable = true;
        }

        /// <summary>
        /// Creates an item based off the scriptable object template.
        /// </summary>
        /// <param name="template"></param>
        public Consumable(ConsumableObject template)
        {
            Template = template;
            Name = template.Name;
            Description = template.Description;
            Type = template.Type;
            Icon = template.Icon;
            IsStackable = template.IsStackable;
            Rarity = template.Rarity;

            healthRestore = template.HealthRestore;
            manaRestore = template.ManaRestore;
            regenTime = template.RegenTime;
        }

        public override bool Use()
        {
            Debug.Log("This is a consumable. Using " + Name + ".");
            return true;
        }

        /// <summary>
        /// Converts the stats to a text form.
        /// </summary>
        /// <returns></returns>
        public override string GetDetailText()
        {
            if (healthRestore <= 0 && manaRestore <= 0) return "";

            string str = "Restores ";
            if (healthRestore > 0)
            {
                str += healthRestore + " health" + (manaRestore > 0 ? " and " : "");
            }
            if (manaRestore > 0)
            {
                str += manaRestore + " mana";
            }
            str += (regenTime > 0 ? " over " + regenTime + " seconds." : ".");
            return str;
        }
    }
}