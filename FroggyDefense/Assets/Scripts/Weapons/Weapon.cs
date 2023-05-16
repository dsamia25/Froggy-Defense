using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    // TODO: Maybe will be used later.
    /// <summary>
    /// Determines a weapon's attack animation.
    /// - Swing will cause a moving attack arc.
    /// - Stab will cause a moving attack forward.
    /// - Slam will cause an immediate damage area.
    /// </summary>
    //public enum WeaponAttackStyle
    //{
    //    Swing,
    //    Stab,
    //    Slam
    //}

    public class Weapon
    {
        [SerializeField] private WeaponObject _template;

        public string Name => _template.name;

        public bool HasMeleeAttack => _template.HasMeleeAttack;
        public float MeleeDamage => _template.MeleeDamage;
        public float MeleeKnockback => _template.MeleeKnockback;
        public float MeleeKnockbackTime => _template.MeleeKnockbackTime;

        public bool HasLunge => _template.HasLunge;
        public float LungeStrength => _template.LungeStrength;
        public float LungeTime => _template.LungeTime;

        public bool HasProjectile => _template.ShootsProjectile;
        public ProjectileInfo Projectile { get; private set; }

        public float AttackCooldown;
        public float CurrAttackCooldown { get; private set; }

        public Weapon(WeaponObject template)
        {
            _template = template;
            Projectile = _template.Projectile;
        }

        /// <summary>
        /// Uses the weapon in the set direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Attack(Character user, Vector2 dir)
        {
            if (CurrAttackCooldown > 0)
            {
                Debug.Log(Name + " on cooldown. " + CurrAttackCooldown.ToString("0.00") + " seconds left.");
                return;
            }
            Debug.Log(user.Name + " using " + Name + " in direction (" + dir + "). HasProjectile: " + HasProjectile.ToString() + ".");

            if (HasMeleeAttack)
            {

            }

            if (HasProjectile)
            {

            }

            if (HasLunge)
            {

            }

            CurrAttackCooldown = AttackCooldown;
        }
    }
}