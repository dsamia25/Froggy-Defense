using System;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Buildings;

namespace FroggyDefense.Weapons
{
    [Serializable]
    public class ProjectileInfo
    {
        [Space]
        [Header("Basic")]
        [Space]
        public ProjectileAppearance Appearance;                                  // The projectile prefab.
        public int ProjectilePoolSize = 8;                                          // Max number of projectiles active in the pool.
        public float Damage = 1f;                                                   // How much damage the projectile does.
        public bool HasProjectileDamageScaling = false;                             // If the projectile damage scales with a particular stat.
        public StatValuePair ProjectileDamageScalingFactor;                         // What stat the projectile damage scales with.
        public DamageType DirectDamageType;                                         // What type of damage the projectile does.
        public float MoveSpeed = 1f;                                                // How fast the projectile moves.

        [Space]
        [Header("Splash")]
        [Space]
        public bool HasSplashDamage = false;
        public float SplashDamage = 0f;                                             // Base splash damage.
        public DamageType SplashDamageType;
        // TODO: Add these effects to Weapon.
        public bool HasSplashDamageScaling = false;
        public StatValuePair SplashDamageScalingFactor;                             // What stat the splash damage scales with.

        public LayerMask SplashLayer = 0;                                           // Which layer is effected by splash damage.
        public float SplashRadius = 0f;                                             // How wide the splash radius is.
        // TODO: Add these effects to Weapon.
        public bool HasMaxSplashTargets = false;                                    // If the amount of enemeis effected by splash damage is capped.
        public float MaxSplashTargets = 100;                                        // The max amount of enemies that can be effected by the weapon splash.


        [Space]
        [Header("Piercing")]
        [Space]
        public bool HasPiercing = false;                                            // If the weapon projectil pierces.
        public int MaxPierces = 0;                                                  // How many times the weapon projectile pierces.

        [Space]
        [Header("Travel Range")]
        [Space]
        public bool HasMaxRange = true;                                             // If the projectile has a maximum range.
        public float MaxRange = 20f;                                                // The maximum projectile range.
        // TODO: Add these effects to Weapon
        public bool HasRangeEffectModifier = false;                                 // If the projectile gets weaker or stronger the further it travels.
        public Vector2 TravelDistanceEffectModifierRange = Vector2.one;             // Range of percents of how much the projectile effect changes over its travel distance.

        [Space]
        [Header("Travel Time")]
        [Space]
        public bool HasMaxTime = true;                                              // If the projectile deactivates after a set time.
        public float TimeLimit = 20f;                                               // The maximum projectile active time.
        // TODO: Add these effects to Weapon
        public bool HasTimeEffectModifier = false;                                  // If the projectile gets weaker or stronger the longer it is out.
        public Vector2 TimeEffectModifierRange = Vector2.one;                       // Range of percents of how much the projectile effect changes over its travel time.

        [Space]
        [Header("Projectile Seeking")]
        [Space]
        public bool HasSeeking = false;                                             // If the projectile seeks out enemies.
        public float SeekingTurnAngle = .05f;                                       // How hard the projectile can course correct towards its target.
        // TODO: Add these effects to Weapon.
        public TargetSetting SeekingTargetPriority = TargetSetting.ClosestTarget;   // Which enemy should be prioritized.

        // TODO: Add these effects to Weapon.
        [Space]
        [Header("Projectile Bounce")]
        [Space]
        public bool HasBounce = false;                                              // If projectile bounces to additional targets.
        public float Bounces = 3;                                                   // How many additional targets the projectile can bounce to.
        public LayerMask BounceTargetLayer = 0;                                     // What layer the bounces can target.
        public TargetSetting BounceTargetPriority = TargetSetting.ClosestTarget;    // Which enemy should be prioritized.
        public float BounceRadius;                                                  // How far the projectile can bounce.
        public float BounceEffectModifier = -.1f;                                   // How much the effect is effected on each bounce.
        public bool OnlyEffectFirstBounce = false;                                  // If the bounce modifier only effects the first bounce or all bounces.
        public bool CanBounceToRepeatTargets = false;                               // If the projectile can bounce to the same target multiple times.

        [Space]
        [Header("Spray Pattern")]
        [Space]
        public bool HasSprayPattern = false;                                        // If the weapon has a spray pattern to modify its shooting.
        public float[] SprayPattern = null;                                         // Each float is an angle that should be added to each shot in a series, modifying its firing angle to make a pattern.
        public float SprayPatternCooldown = 1f;                                     // How long until the spray pattern index is reset.

        [Space]
        [Header("Burst Fire")]
        [Space]
        public bool BurstFireEnabled = false;                                       // If the weapon should use the input burst fire specs by default.
        public int BurstFireAmount = 1;                                             // How many shots will be fired at once.
        public float BurstDelay = .25f;                                             // How many second between each shot in the burst.
    }
}