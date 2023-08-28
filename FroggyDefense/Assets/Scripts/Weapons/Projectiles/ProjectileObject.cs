using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Buildings;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Weapons
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Weapons/New Projectile")]
    public class ProjectileObject: ScriptableObject
    {
        public GameObject Vfx;                                                      // The projectile's prefab.
        public int ProjectilePoolSize = 8;                                          // Max number of projectiles active in the pool.
        public float MoveSpeed = 1f;                                                // How fast the projectile moves.
        public DamageActionArgs DamageArgs;
        public SpellAction[] OnHitActions;
        public SpellAction[] OnExpireActions;

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
        public bool HasMaxTime = true;                                              // If the projectile deactivates after a set time.
        public float TimeLimit = 20f;                                               // The maximum projectile active time.
        // TODO: Add these effects to Weapon
        public bool HasTimeEffectModifier = false;                                  // If the projectile gets weaker or stronger the longer it is out.
        public Vector2 TimeEffectModifierRange = Vector2.one;                       // Range of percents of how much the projectile effect changes over its travel time.

        [Space]
        [Header("Projectile Seeking")]
        public bool HasSeeking = false;                                             // If the projectile seeks out enemies.
        public float SeekingTurnAngle = .05f;                                       // How hard the projectile can course correct towards its target.
        // TODO: Add these effects to Weapon.
        public TargetSetting SeekingTargetPriority = TargetSetting.ClosestTarget;   // Which enemy should be prioritized.

        // TODO: Add these effects to Weapon.
        [Space]
        [Header("Projectile Bounce")]
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
        public bool HasSprayPattern = false;                                        // If the weapon has a spray pattern to modify its shooting.
        public float[] SprayPattern = null;                                         // Each float is an angle that should be added to each shot in a series, modifying its firing angle to make a pattern.
        public float SprayPatternCooldown = 1f;                                     // How long until the spray pattern index is reset.

        [Space]
        [Header("Burst Fire")]
        public bool BurstFireEnabled = false;                                       // If the weapon should use the input burst fire specs by default.
        public int BurstFireAmount = 1;                                             // How many shots will be fired at once.
        public float BurstDelay = .25f;                                             // How many second between each shot in the burst.
    }
}