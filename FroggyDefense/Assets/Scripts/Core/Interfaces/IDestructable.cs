using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core
{
    public interface IDestructable
    {
        /// <summary>
        /// Damages the object by the given amount.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage);

        /// <summary>
        /// Damages the object with the given damage action.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(DamageAction damage);

        // TODO: Merge ApplyDot and ApplyStatusEffect into the same method.
        /// <summary>
        /// Applies a damage over time effect.
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyDot(DamageOverTimeEffect dot);

        /// <summary>
        /// Applies a status effect.
        /// </summary>
        /// <param name="status"></param>
        public void ApplyStatusEffect(StatusEffect status)
        {

        }

        /// <summary>
        /// Resolves any death effects for the object.
        /// </summary>
        public void Die();
    }

    // TODO: Not sure if it should be a struct or a class.
    /// <summary>
    /// Holds the info on how much damage is being dealt and what kind of damage it was.
    /// </summary>
    public struct DamageAction
    {
        public float Damage;
        public DamageType Type;
        public Character Attacker;

        public DamageAction(Character attacker, float damage, DamageType type)
        {
            Attacker = attacker;
            Damage = damage;
            Type = type;
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