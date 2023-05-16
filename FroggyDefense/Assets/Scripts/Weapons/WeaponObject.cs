using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapons/New Weapon")]
    public class WeaponObject : ScriptableObject
    {
        // TODO: Reorder these effects in the list.
        [Space]
        [Header("Direct Damage")]
        [Space]
        // TODO: Add these effects to Weapon.
        public StatType m_DirectDamageScalingStat = StatType.strength;              // Which stat effects the weapon's direct damage.
        public float m_DirectDamageScalingFactor = 1f;                              // How much the stat effects the weapon's direct damage.

        [Space]
        [Header("Melee Stats")]
        [Space]
        [SerializeField] private bool _hasMeleeAttack;                              // If the weapon has a melee attack component.
        [SerializeField] private float _meleeDamage;                                // How much damage the melee attack does.
        [SerializeField] private float _meleeKnockback = 1;                         // How much the attack knock enemies back.
        [SerializeField] private float _meleeKnockbackTime = .25f;                  // How long enemies are stuck in the knockback.
        [SerializeField] private float _meleeRange;                                 // How far the weapon attack range is.
        public bool HasMeleeAttack => _hasMeleeAttack;
        public float MeleeDamage => _meleeDamage;
        public float MeleeKnockback => _meleeKnockback;
        public float MeleeKnockbackTime => _meleeKnockbackTime;
        public float MeleeRange => _meleeRange;

        [Space]
        [Header("Lunge Stats")]
        [Space]
        [SerializeField] private bool _hasLunge;                                    // If the user lunges on attack.
        [SerializeField] private float _lungeStrength;                              // How much the user lunges forward.
        [SerializeField] private float _lungeTime;                                  // How long the lunge lasts for.
        [SerializeField] private float _lungeCooldown;                              // How long between the projectile will be shot again.
        public bool HasLunge => _hasLunge;
        public float LungeStrength => _lungeStrength;
        public float LungeTime => _lungeTime;
        public float LungeCooldown => _lungeCooldown;

        [Space]
        [Header("Projectile")]
        [Space]
        [SerializeField] private bool _shootsProjectile = true;                     // If the weapon shoots a projectile.
        [SerializeField] private float _projectileCooldown = .25f;                  // How long between the projectile will be shot again.
        [SerializeField] private ProjectileInfo _projectile;                        // If the weapon has a melee attack component.
        public bool ShootsProjectile => _shootsProjectile;
        public float ProjectileCooldown => _projectileCooldown;
        public ProjectileInfo Projectile => _projectile;
        
    }
}