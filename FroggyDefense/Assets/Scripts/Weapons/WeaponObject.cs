using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Buildings;

namespace FroggyDefense.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapons/New Weapon")]
    public class WeaponObject : ScriptableObject
    {
        [Space]
        [Header("Direct Damage")]
        [Space]
        public float m_WeaponDamage = 1;                                            // Base weapon damage.
        // TODO: Add these effects to Weapon.
        public StatType m_DirectDamageScalingStat = StatType.strength;              // Which stat effects the weapon's direct damage.
        public float m_DirectDamageScalingFactor = 1f;                              // How much the stat effects the weapon's direct damage.

        [Space]
        [Header("Splash Damage")]
        [Space]
        public bool m_HasSplashDamage = false;
        public float m_SplashDamage = 0f;                                           // Base splash damage.
        // TODO: Add these effects to Weapon.
        public StatType m_SplashDamageScalingStat = StatType.strength;              // Which stat effects the weapon's splash damage.
        public float m_SplashDamageScalingFactor = 1f;                              // How much the stat effects the weapon's splash damage.
        public LayerMask m_SplashLayer = 0;                                         // Which layer is effected by splash damage.
        public float m_SplashRadius = 0f;                                           // How wide the splash radius is.
        // TODO: Add these effects to Weapon.
        public bool m_HasMaxSplashTargets = false;                                  // If the amount of enemeis effected by splash damage is capped.
        public float m_MaxSplashTargets = 100;                                      // The max amount of enemies that can be effected by the weapon splash.

        [Space]
        [Header("Cooldown")]
        [Space]
        public float m_AttackCooldown = .25f;                                       // How long it takes to attack again.

        [Space]
        [Header("Projectile")]
        [Space]
        public GameObject m_ProjectilePrefab = null;                                // The projectile prefab.
        public int m_ProjectilePoolSize = 8;                                        // Max number of projectiles active in the pool.
        public float m_ProjectileSpeed = 1f;                                        // How fase the projectile moves.

        [Space]
        [Header("Projectile Piercing")]
        [Space]
        public bool m_HasPiercing = false;                                          // If the weapon projectil pierces.
        public int m_PiercingNumber = 0;                                            // How many times the weapon projectile pierces.

        [Space]
        [Header("Projectile Travel Range")]
        [Space]
        public bool m_HasMaxRange = true;                                           // If the projectile has a maximum range.
        public float m_MaxRange = 20f;                                              // The maximum projectile range.
        // TODO: Add these effects to Weapon
        public bool m_HasRangeEffectModifier = false;                               // If the projectile gets weaker or stronger the further it travels.
        public Vector2 m_TravelDistanceEffectModifierRange = Vector2.one;           // Range of percents of how much the projectile effect changes over its travel distance.

        [Space]
        [Header("Projectile Travel Time")]
        [Space]
        public bool m_HasMaxTime = true;                                            // If the projectile deactivates after a set time.
        public float m_TimeLimit = 20f;                                             // The maximum projectile active time.
        // TODO: Add these effects to Weapon
        public bool m_HasTimeEffectModifier = false;                                // If the projectile gets weaker or stronger the longer it is out.
        public Vector2 m_TimeEffectModifierRange = Vector2.one;                     // Range of percents of how much the projectile effect changes over its travel time.

        [Space]
        [Header("Projectile Seeking")]
        [Space]
        public bool m_HasSeeking = false;                                           // If the projectile seeks out enemies.
        public float m_SeekingTurnAngle = .05f;                                     // How hard the projectile can course correct towards its target.
        // TODO: Add these effects to Weapon.
        public TargetSetting m_SeekingTargetPriority = TargetSetting.ClosestTarget; // Which enemy should be prioritized.

        // TODO: Add these effects to Weapon.
        [Space]
        [Header("Projectile Bounce")]
        [Space]
        public bool m_HasBounce = false;                                            // If projectile bounces to additional targets.
        public float m_Bounces = 3;                                                 // How many additional targets the projectile can bounce to.
        public LayerMask m_BounceLayer = 0;                                         // What layer the bounces can target.
        public TargetSetting m_BounceTargetPriority = TargetSetting.ClosestTarget;  // Which enemy should be prioritized.
        public float m_BounceRadius;                                                // How far the projectile can bounce.
        public float m_BounceEffectModifier = -.1f;                                 // How much the effect is effected on each bounce.
        public bool m_OnlyEffectFirstBounce = false;                                // If the bounce modifier only effects the first bounce or all bounces.
        public bool m_CanBounceToRepeatTargets = false;                             // If the projectile can bounce to the same target multiple times.

        [Space]
        [Header("Spray Pattern")]
        [Space]
        public bool m_HasSprayPattern = false;                                      // If the weapon has a spray pattern to modify its shooting.
        public float[] m_SprayPattern = null;                                       // Each float is an angle that should be added to each shot in a series, modifying its firing angle to make a pattern.
        public float m_SprayPatternCooldown = 1f;                                   // How long until the spray pattern index is reset.

        [Space]
        [Header("Burst Fire")]
        [Space]
        public bool m_BurstFireEnabled = false;                                     // If the weapon should use the input burst fire specs by default.
        public int m_BurstFireAmount = 1;                                           // How many shots will be fired at once.
        public float m_BurstDelay = .25f;                                           // How many second between each shot in the burst.
    }
}