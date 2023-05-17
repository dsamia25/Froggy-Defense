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
        public DamageType MeleeDamageType => _template.MeleeDamageType;
        public bool HasMeleeDamageScaling => _template.HasMeleeDamageScaling;
        public StatValuePair MeleeDamageScalingFactor => _template.MeleeDamageScalingFactor;
        public float MeleeKnockback => _template.MeleeKnockback;
        public float MeleeKnockbackTime => _template.MeleeKnockbackTime;

        public bool HasLunge => _template.HasLunge;
        public float LungeStrength => _template.LungeStrength;
        public float LungeTime => _template.LungeTime;

        public bool HasProjectile => _template.ShootsProjectile;
        public float ProjectileCooldown => _template.ProjectileCooldown;
        public ProjectileInfo Projectile { get; private set; }

        public float AttackCooldown;
        public float CurrAttackCooldown { get; private set; }

        private Character _user;

        public Weapon(WeaponObject template)
        {
            _template = template;
            Projectile = _template.Projectile;
        }

        public void Equip(Character user)
        {
            this._user = user;
        }

        /// <summary>
        /// Returns the calculated scaling factor
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetStatScaling(StatValuePair stat)
        {
            return stat.Value * _user.Stats.GetTotalStat(stat.Stat);
        }
    }
}