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
        /// Applies an overtime effect to the thing.
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyDebuff(Debuff effect);

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

        public DamageAction(float damage, DamageType type)
        {
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