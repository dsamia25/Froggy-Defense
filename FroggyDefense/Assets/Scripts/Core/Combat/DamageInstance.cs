using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Holds the info on how much damage is being dealt and what kind of damage it was.
    /// </summary>
    public struct DamageInstance
    {
        [System.Serializable]
        public struct Args
        {
            public float Damage;                            // How much damage the spell does.
            public float SpellPowerRatio;                   // Extra damage based on percent of spell power.
            public DamageType SpellDamageType;              // What kind of damage is applied (If applicable).
            public float CritChanceModifier;                // Extra criticial strike chance.
            public float CritBonusModifier;                 // Extra critical strike bonus modifier.

            public Args(float damage, float spellPowerRatio, DamageType type, float critChanceModifier, float critBonusModifier)
            {
                Damage = damage;
                SpellPowerRatio = spellPowerRatio;
                SpellDamageType = type;
                CritChanceModifier = critChanceModifier;
                CritBonusModifier = critBonusModifier;
            }
        }
        
        public float Damage;
        public DamageType Type;
        public Character Attacker;
        public bool IsCriticalStrike;

        public DamageInstance(Character attacker, float damage, DamageType type, bool isCriticalStrike)
        {
            Attacker = attacker;
            Damage = damage;
            Type = type;
            IsCriticalStrike = isCriticalStrike;
        }

        /// <summary>
        /// Creates a damage instance using just the attacker and damage args.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DamageInstance CreateDamageInstance(Character attacker, Args args)
        {
            return CreateDamageInstance(attacker, args.Damage, args.SpellPowerRatio, args.SpellDamageType, args.CritChanceModifier, args.CritBonusModifier);
        }

        /// <summary>
        /// Creates a damage instance.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="baseDamage"></param>
        /// <param name="spellPowerRatio"></param>
        /// <param name="type"></param>
        /// <param name="critChanceModifier"></param>
        /// <param name="critBonusModifier"></param>
        /// <returns></returns>
        public static DamageInstance CreateDamageInstance(Character attacker, float baseDamage, float spellPowerRatio, DamageType type, float critChanceModifier, float critBonusModifier)
        {
            float damage = baseDamage + spellPowerRatio * attacker.GetSpellPower(type);
            bool crit = false;
            if (CritRoll(attacker.CritChance, critChanceModifier))
            {
                // TODO: Invoke a Character.CriticalStrike event.
                damage *= critBonusModifier * attacker.CritBonus;
                crit = true;
            }
            return new DamageInstance(attacker, damage, type, crit);
        }

        /// <summary>
        /// Rolls for a critical strike.
        /// </summary>
        /// <returns></returns>
        public static bool CritRoll(float critChance, float critChanceModifier)
        {
            float roll = Random.value;
            return roll < critChanceModifier * critChance;
        }
    }
}