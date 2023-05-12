using System.Collections;
using UnityEngine;
using FroggyDefense.Movement;

namespace FroggyDefense.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform m_FireLoc = null;                        // Location where the projectiles should be shot from.
        [SerializeField] private Vector2 m_FireDir = Vector2.zero;                  // The direction the projectiles should be shot in.
        public IUseWeapon User;

        [SerializeField] private WeaponObject m_StartingWeapon = null;              // If the component should copy these default values.
        
        [SerializeField] private ProjectilePool m_ProjectilePool = null;
        private Transform _projectilePoolParent = null;

        [Space]
        [Header("Damage")]
        [Space]
        [SerializeField] private float _weaponDamage = 1f;
        [SerializeField] private bool _hasSplashDamage = false;
        [SerializeField] private float _splashDamage = 0f;
        [SerializeField] private LayerMask _splashLayer = 0;
        [SerializeField] private float _splashRadius = 0;
        [SerializeField] private float _userDirectDamageScaleFactor = 1f;           // What percent of the user's direct damage stat is added to the damage calculation.
        [SerializeField] private float _userSplashDamageScaleFactor = 1f;           // What percent of the user's splash damage stat is added to the damage calculation.
        public float WeaponDamage => _weaponDamage;                                 // How much damage the weapon does.
        public float UserDirectDamageScaleFactor => _userDirectDamageScaleFactor;
        public float UserSplashDamageScaleFactor => _userSplashDamageScaleFactor;
        public bool m_HasSplashDamage => _hasSplashDamage;
        public float m_SplashDamage => _splashDamage;
        public LayerMask m_SplashLayer => _splashLayer;
        public float m_SplashRadius => _splashRadius;

        [Space]
        [Header("Cooldown")]
        [Space]
        [SerializeField] private float _attackCooldown = .1f;
        public float AttackCooldown { get => _attackCooldown; }                     // How long it takes to shoot again.

        [Space]
        [Header("Melee Stats")]
        [Space]
        [SerializeField] private float _weaponRange;                                // How far the weapon attack range is.
        [SerializeField] private bool _hasLunge;                                    // If the user lunges on attack.
        [SerializeField] private float _lungeStrength;                              // How much the user lunges forward.
        [SerializeField] private float _lungeTime;                                  // How long the lunge lasts for.
        public float WeaponRange => _weaponRange;
        public bool HasLunge => _hasLunge;
        public float LungeStrength => _lungeStrength;
        public float LungeTime => _lungeTime;

        [Space]
        [Header("Projectile Stats")]
        [Space]
        [SerializeField] private bool _shootsProjectile = true;                     // If the weapon shoots a projectile.
        [SerializeField] private GameObject _projectilePrefab = null;               // The basic projectile to use.
        [SerializeField] private int _projectilePoolSize = 8;                       // The maximum size of the pool.
        [SerializeField] private float _projectileSpeed = 1;                        // How fase the projectile moves.
        [SerializeField] private bool _hasPiercing = false;                         // If the weapon projectile pierces.
        [SerializeField] private int _piercingNumber = 0;                           // How many times the weapon projectile pierces.
        [SerializeField] private bool _hasMaxRange = true;                          // If the projectile deactivates after a set time.
        [SerializeField] private float _maxRange = 20f;                             // The maximum projectile range.
        [SerializeField] private bool _hasMaxTime = true;                           // If the projectile deactivates after a set time.
        [SerializeField] private float _timeLimit = 20f;                            // The maximum projectile active time.
        [SerializeField] private bool _hasSeeking = false;                          // If the projectile seeks out enemies.
        [SerializeField] private float _seekingTurnAngle = .05f;                    // How hard the projectile can course correct towards its target.
        [SerializeField] private bool ShootsProjectile => _shootsProjectile;
        public float m_ProjectileSpeed => _projectileSpeed;
        public bool m_HasPiercing => _hasPiercing;                  
        public int m_PiercingNumber => _piercingNumber;
        public bool m_HasMaxRange => _hasMaxRange;
        public float m_MaxRange => _maxRange;
        public bool m_HasMaxTime => _hasMaxTime;
        public float m_TimeLimit => _timeLimit;           
        public bool m_HasSeeking => _hasSeeking;                   
        public float m_SeekingTurnAngle => _seekingTurnAngle;            

        [Space]
        [Header("Spray Pattern")]
        [Space]
        [SerializeField] private bool m_HasSprayPattern = false;                    // If the weapon has a spray pattern to modify its shooting.
        [SerializeField] private float[] m_SprayPattern = null;                     // Each float is an angle that should be added to each shot in a series, modifying its firing angle to make a pattern.
        [SerializeField] private int m_SprayPatternIndex = 0;                       // The current position in the spray pattern array.
        [SerializeField] private float m_SprayPatternCooldown = 1f;                 // How long until the spray pattern index is reset.
        private float m_CurrentSprayPatternCooldown = 0f;                           // The current cooldown before the spray pattern resets.

        [Space]
        [Header("Burst Fire")]
        [Space]
        [SerializeField] private bool m_BurstFireEnabled = false;                   // If the weapon should use the input burst fire specs by default.
        [SerializeField] private int m_BurstFireAmount = 1;                         // How many shots will be fired at once.
        [SerializeField] private float m_BurstDelay = .25f;                         // How many second between each shot in the burst.

        private Rigidbody2D rb;
        private ObjectController controller;
        private float directDamageSnapshot = 0f;
        private float splashDamageSnapshot = 0f;
        private bool isShooting = false;                                            // Key to if a new shot can be fired while another shot is being fired.
        private float currCooldown = 0f;                                            // The current cooldown timer.

        // **************************************************
        // Initialization
        // **************************************************

        private void Awake()
        {
            if (User == null)
            {
                User = gameObject.GetComponent<IUseWeapon>();
            }

            if (rb == null)
            {
                rb = gameObject.GetComponent<Rigidbody2D>();
            }

            if (controller == null)
            {
                controller = gameObject.GetComponent<ObjectController>();
            }
        }

        private void Start()
        {
            if (m_FireLoc == null)
            {
                m_FireLoc = transform;
            }

            // If the component should copy these default values instead of using input custom values.
            if (m_StartingWeapon != null)
            {
                Equip(m_StartingWeapon);
            }

            _projectilePoolParent = new GameObject(gameObject.name + "ProjectilePool").transform;
            m_ProjectilePool = new ProjectilePool(_projectilePrefab, _projectilePoolParent, _projectilePoolSize);
        }

        // **************************************************
        // Update
        // **************************************************

        private void Update()
        {
            currCooldown -= Time.deltaTime;

            if (m_CurrentSprayPatternCooldown <= 0)
            {
                m_SprayPatternIndex = 0;
            }
            else
            {
                m_CurrentSprayPatternCooldown -= Time.deltaTime;
            }

            // TODO: Copy Recharge Bar from previous version.
        }

        // **************************************************
        // Methods
        // **************************************************

        /// <summary>
        /// Equips the new weapon specs.
        /// </summary>
        /// <param name="weapon"></param>
        public void Equip(WeaponObject template)
        {
            _shootsProjectile = ShootsProjectile;
            _projectilePrefab = template.m_ProjectilePrefab;
            _projectilePoolSize = template.m_ProjectilePoolSize;
            _projectileSpeed = template.m_ProjectileSpeed;
            _hasPiercing = template.m_HasPiercing;
            _piercingNumber = template.m_PiercingNumber;
            _hasMaxRange = template.m_HasMaxRange;
            _maxRange = template.m_MaxRange;
            _hasMaxTime = template.m_HasMaxTime;
            _timeLimit = template.m_TimeLimit;
            _hasSeeking = template.m_HasSeeking;
            _seekingTurnAngle = template.m_SeekingTurnAngle;
            if (_shootsProjectile)
            {
                m_ProjectilePool.Clear();
                m_ProjectilePool = new ProjectilePool(_projectilePrefab, _projectilePoolParent, _projectilePoolSize);
            }

            _weaponRange = template.WeaponRange;
            _hasLunge = template.HasLunge;
            _lungeStrength = template.LungeStrength;
            _lungeTime = template.LungeTime;

            _weaponDamage = template.m_WeaponDamage;
            _hasSplashDamage = template.m_HasSplashDamage;
            _splashDamage = template.m_SplashDamage;
            _splashLayer = template.m_SplashLayer;
            _splashRadius = template.m_SplashRadius;
            _userDirectDamageScaleFactor = template.m_DirectDamageScalingFactor;
            _userSplashDamageScaleFactor = template.m_SplashDamageScalingFactor;

            m_HasSprayPattern = template.m_HasSprayPattern;
            m_SprayPattern = template.m_SprayPattern;
            m_SprayPatternCooldown = template.m_SprayPatternCooldown;

            m_BurstFireEnabled = template.m_BurstFireEnabled;
            m_BurstFireAmount = template.m_BurstFireAmount;
            m_BurstDelay = template.m_BurstDelay;

            _attackCooldown = template.m_AttackCooldown;
        }

        /// <summary>
        /// Shoots the projectile in the default direction.
        /// Used for the player's shooting button.
        /// </summary>
        public void Shoot()
        {
            Shoot(m_FireDir);
        }

        /// <summary>
        /// Shoots the weapon in the input direction. Uses a single or burst fire depending on weapon settings.
        /// May not be able to shoot if called when the weapon is on cooldown or is currently shooting a burst.
        /// 
        /// Returns if the weapon actually shot or not.
        /// </summary>
        /// <param name="FireDir"></param>
        /// <returns></returns>
        public bool Shoot(Vector2 FireDir)
        {
            if (currCooldown <= 0 && !isShooting)
            {
                directDamageSnapshot = CalculateDirectDamage();
                splashDamageSnapshot = CalculateSplashDamage();

                // Shoot the weapon.
                if (m_BurstFireEnabled)
                {
                    StartCoroutine(FireBurst(FireDir, m_BurstFireAmount, m_BurstDelay));
                }
                else
                {
                    StartCoroutine(FireBurst(FireDir, 1, 0));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Internal system for firing projectiles in the input direction.
        /// </summary>
        private void Fire(Vector2 FireDir)
        {
            if (_hasLunge)
            {
                controller.Lunge(FireDir, _lungeStrength, _lungeTime, .75f * _lungeTime);
            }

            if (_shootsProjectile)
            {
                Projectile projectile = m_ProjectilePool.Get();
                projectile.transform.position = m_FireLoc.position;

                if (m_HasSprayPattern)
                {
                    projectile.Shoot(this, RotateVector(FireDir, m_SprayPattern[m_SprayPatternIndex++ % m_SprayPattern.Length]), directDamageSnapshot, splashDamageSnapshot);
                }
                else
                {
                    projectile.Shoot(this, FireDir, directDamageSnapshot, splashDamageSnapshot);
                }

                m_CurrentSprayPatternCooldown = m_SprayPatternCooldown;
            }
        }

        /// <summary>
        /// Shoots the weapon in the input direction the input number of times.
        /// </summary>
        private IEnumerator FireBurst(Vector2 FireDir, int burstAmount, float burstDelay)
        {
            isShooting = true;  // Takes key to prevent repeat fires during burst.
            for (int i = 0; i < burstAmount; i++)
            {
                Fire(FireDir);
                yield return new WaitForSeconds(burstDelay);
            }
            currCooldown = _attackCooldown;                             // Resets cooldown.

            // TODO: Copy the recharge bar.
            //// Toggle the recharge bar.
            //if (m_RechargeBar != null)
            //{
            //    m_RechargeBar.gameObject.SetActive(true);
            //}

            isShooting = false; // Returns key to allow shooting again after cooldown is over.
        }

        public float CalculateDirectDamage()
        {
            float damage = WeaponDamage + (UserDirectDamageScaleFactor * User.GetDirectDamage());
            return damage;
        }

        public float CalculateSplashDamage()
        {
            float damage = WeaponDamage + (UserSplashDamageScaleFactor * User.GetSplashDamage());
            return damage;
        }

        /// <summary>
        /// Returns the input vector rotated by the input degrees.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vector2 RotateVector(Vector2 dir, float angle)
        {
            float baseAngle = Vector2.Angle(Vector2.up, dir);
            float newAngle = baseAngle + angle + 90;

            float x = dir.magnitude * Mathf.Cos(Mathf.Deg2Rad * newAngle);
            float y = dir.magnitude * Mathf.Sin(Mathf.Deg2Rad * newAngle);

            Vector2 newDir = new Vector2(x, y);
            Debug.Log("RotateVectorDegrees(" + angle + ", " + newAngle + ")\nFIREDIR = (" + dir.ToString() + "). BASE ANGLE = " + baseAngle + " degrees. Magnitude = " + dir.magnitude + "\n"
                + "NEWDIR = (x = " + x + ", y = " + y + "), (" + newDir.ToString() + "). ANGLE = " + Vector2.Angle(Vector2.up, newDir) + " degrees. Magnitude = " + newDir.magnitude);
            return newDir;
        }
    }
}