using UnityEngine;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core
{
    public interface IDestructable
    {

        /// <summary>
        /// Damages the object by the given amount.
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(float damage);

        /// <summary>
        /// Damages the object with the given damage action.
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(DamageAction damage);

        /// <summary>
        /// Applies a status effect.
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(AppliedEffect effect);

        /// <summary>
        /// Knocks back the unit in the given direction with the set strength.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="strength"></param>
        public void KnockBack(Vector2 dir, float strength, float knockBackTime, float moveLockTime);

        /// <summary>
        /// Resolves any death effects for the object.
        /// </summary>
        public virtual void Die()
        {

        }
    }

    /// <summary>
    /// Holds the info on how much damage is being dealt and what kind of damage it was.
    /// </summary>
    public struct DamageAction
    {
        public float Damage;
        public DamageType Type;
        public Character Attacker;
        public bool IsCriticalStrike;

        public DamageAction(Character attacker, float damage, DamageType type, bool isCriticalStrike)
        {
            Attacker = attacker;
            Damage = damage;
            Type = type;
            IsCriticalStrike = isCriticalStrike;
        }

        /// <summary>
        /// Creates a damage action using just the attacker and damage args.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DamageAction CreateDamageAction(Character attacker, DamageActionArgs args)
        {
            return CreateDamageAction(attacker, args.Damage, args.SpellPowerRatio, args.SpellDamageType, args.CritChanceModifier, args.CritBonusModifier);
        }

        /// <summary>
        /// Creates a damage action.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="baseDamage"></param>
        /// <param name="spellPowerRatio"></param>
        /// <param name="type"></param>
        /// <param name="critChanceModifier"></param>
        /// <param name="critBonusModifier"></param>
        /// <returns></returns>
        public static DamageAction CreateDamageAction(Character attacker, float baseDamage, float spellPowerRatio, DamageType type, float critChanceModifier, float critBonusModifier)
        {
            float damage = baseDamage + spellPowerRatio * attacker.GetSpellPower(type);
            bool crit = false;
            if (CritRoll(attacker.CritChance, critChanceModifier))
            {
                // TODO: Invoke a Character.CriticalStrike event.
                damage *= critBonusModifier * attacker.CritBonus;
                crit = true;
            }
            return new DamageAction(attacker, damage, type, crit);
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

    [System.Serializable]
    public struct DamageActionArgs
    {
        public float Damage;                            // How much damage the spell does.
        public float SpellPowerRatio;                   // Extra damage based on percent of spell power.
        public DamageType SpellDamageType;              // What kind of damage is applied (If applicable).
        public float CritChanceModifier;                // Extra criticial strike chance.
        public float CritBonusModifier;                 // Extra critical strike bonus modifier.

        public DamageActionArgs(float damage, float spellPowerRatio, DamageType type, float critChanceModifier, float critBonusModifier)
        {
            Damage = damage;
            SpellPowerRatio = spellPowerRatio;
            SpellDamageType = type;
            CritChanceModifier = critChanceModifier;
            CritBonusModifier = critBonusModifier;
        }
    }

    /// <summary>
    /// Enum of all kinds of damage.
    /// </summary>
    public enum DamageType
    {
        Physical,
        Poison,
        Bleed,
        Fire,
        Frost,
        Spirit
    }
}