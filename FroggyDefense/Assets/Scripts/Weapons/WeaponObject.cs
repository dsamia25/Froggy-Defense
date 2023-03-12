using UnityEngine;

namespace FroggyDefense.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapons/New Weapon")]
    public class WeaponObject : ScriptableObject
    {
        [Space]
        [Header("Damage")]
        [Space]
        public float m_WeaponDamage = .25f;   // How much damage the weapon does.
        public float m_UserDirectDamageScaleFactor = 1f;    // How much of the user's direct damage stat is used in the damage calculation.
        public float m_UserSplashDamageScaleFactor = 1f;    // How much of the user's splash damage stat is used in the damage calculation.

        [Space]
        [Header("Cooldown")]
        [Space]
        public float m_ShootCooldown = .25f;   // How long it takes to shoot again.

        [Space]
        [Header("Projectile")]
        [Space]
        public GameObject m_ProjectilePrefab = null;
        public int m_ProjectilePoolSize = 8;

        [Space]
        [Header("Spray Pattern")]
        [Space]
        public bool m_HasSprayPattern = false;    // If the weapon has a spray pattern to modify its shooting.
        public float[] m_SprayPattern = null; // Each float is an angle that should be added to each shot in a series, modifying its firing angle to make a pattern.
        public float m_SprayPatternCooldown = 1f; // How long until the spray pattern index is reset.

        [Space]
        [Header("Burst Fire")]
        [Space]
        public bool m_BurstFireEnabled = false;    // If the weapon should use the input burst fire specs by default.
        public int m_BurstFireAmount = 1;  // How many shots will be fired at once.
        public float m_BurstDelay = .25f; // How many second between each shot in the burst.
    }
}