using UnityEngine;
using Core.Spells;

namespace Core.Combat
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
        public abstract void TakeDamage(DamageInstance damage);

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
        /// <param name="knockBackTime"></param>
        /// <param name="moveLockTime"></param>
        public void KnockBack(Vector2 dir, float strength, float knockBackTime, float moveLockTime);

        /// <summary>
        /// Gets the GameObject this component is on.
        /// </summary>
        /// <returns></returns>
        public abstract GameObject GetGameObject();

        /// <summary>
        /// Resolves any death effects for the object.
        /// </summary>
        public virtual void Die()
        {
            
        }
    }
}