namespace FroggyDefense.Core
{
    // TODO: Change the TakeDamage method to have a DamageAction input instead of just a float.
    public interface IDestructable
    {
        /// <summary>
        /// Damages the object by the given amount.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage);

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
        public float damage;
        public DamageType type;

        public DamageAction(float _damage, DamageType _type)
        {
            this.damage = _damage;
            this.type = _type;
        }
    }

    /// <summary>
    /// Enum of all kinds of damage.
    /// </summary>
    public enum DamageType
    {
        Direct,
        Splash,
        True
    }
}