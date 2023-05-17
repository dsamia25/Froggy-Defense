using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private Character Caster;                                       // Who shot the projectile.
        private Weapon _weapon;                                         // The weapon that fired this projectile.
        [SerializeField] private Vector2 m_StorageLoc;                  // Where the projectile is stored.

        private Vector2 m_ShootPos = Vector2.zero;                      // Where the projectile was shot from last. Used to calculate distance.
        private float m_TimeCounter = 0f;                               // Counts how long the projectile has been active.
        private int _pierces = 0;                                       // How many targets this projectile has pierced this launch.

        /*
         * TODO: Make a seeking feature.
         * 
         * - On Shoot, select closest enemy target.
         * - In Update, adjust velocity to turn towards the selected target, a larger SeekingTurnAngle will allow for greater maneuverability (Spelling lol).
         * - Seeking targets should have a Max Time always set.
         */
        
        private Rigidbody2D rb = null;  // The projectile Rigidbody2D.

        private IDestructable m_PrimaryTarget = null;

        // **************************************************
        // Init
        // **************************************************

        private void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            rb.velocity = Vector2.zero;

            if (m_StorageLoc == null)
            {
                m_StorageLoc = Vector2.zero;
            }
        }

        // **************************************************
        // Update
        // **************************************************

        private void FixedUpdate()
        {
            // Only disable after max range if set to.
            if (_weapon.Projectile.HasMaxRange)
            {
                // Return the projectile after it's traveled out of range.
                if (Vector2.Distance(m_ShootPos, transform.position) >= _weapon.Projectile.MaxRange)
                {
                    // Include any explosion setting.
                    Explode();
                }
            }

            if (_weapon.Projectile.HasMaxTime)
            {
                // Return the projectile after it's traveled out of range.
                if (m_TimeCounter <= 0)
                {
                    // Include any explosion setting.
                    Explode();
                } else
                {
                    m_TimeCounter -= Time.fixedDeltaTime;
                }
            }

            if (_weapon.Projectile.HasSeeking)
            {
                // TODO: Implement some sort of way to track after the target using the m_SeekingTurnAngle.
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        public void Shoot(Weapon weapon, Vector2 dir)
        {
            _weapon = weapon;
            _pierces = weapon.Projectile.MaxPierces;

            m_PrimaryTarget = null;
            m_ShootPos = transform.position;
            m_TimeCounter = _weapon.Projectile.TimeLimit;

            gameObject.SetActive(true);
            rb.velocity = _weapon.Projectile.MoveSpeed * dir.normalized;
        }

        /// <summary>
        /// Returns the projectile to its starting position and freezes it.
        /// </summary>
        public void Return()
        {
            rb.velocity = Vector2.zero;
            transform.position = m_StorageLoc;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Causes the projectile to do any of its exploding settings and returns the projectile to its resting location.
        /// Should be called for any situation where the projectile should be destroyed.
        /// </summary>
        public void Explode()
        {
            if (_weapon.Projectile.HasSplashDamage)
            {
                Collider2D[] targetsHit = Physics2D.OverlapCircleAll(transform.position, _weapon.Projectile.SplashRadius, (_weapon.Projectile.SplashLayer == 0 ? gameObject.layer : _weapon.Projectile.SplashLayer));
                foreach (Collider2D collider in targetsHit)
                {
                    IDestructable destructable = null;
                    if ((destructable = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        if (destructable != m_PrimaryTarget)
                        {
                            destructable.TakeDamage(new DamageAction(Caster, _weapon.Projectile.SplashDamage + (_weapon.Projectile.HasSplashDamageScaling ? _weapon.GetStatScaling(_weapon.Projectile.SplashDamageScalingFactor) : 0), _weapon.Projectile.SplashDamageType));
                        }
                    }
                }
            }
            Return();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDestructable destructable = null;
            if ((destructable = collision.gameObject.GetComponent<IDestructable>()) != null)
            {
                destructable.TakeDamage(new DamageAction(Caster, _weapon.Projectile.Damage + (_weapon.Projectile.HasProjectileDamageScaling ? _weapon.GetStatScaling(_weapon.Projectile.ProjectileDamageScalingFactor) : 0), _weapon.Projectile.DirectDamageType));
                m_PrimaryTarget = destructable;

                // Explode if it doesn't have piercing or if it's done with piercing.
                if (!_weapon.Projectile.HasPiercing || (_weapon.Projectile.HasPiercing && --_pierces < 0))
                {
                    Explode();
                }
            }
        }

        /// <summary>
        /// Draws the explosion radius in the editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (_weapon.Projectile.HasSplashDamage)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _weapon.Projectile.SplashRadius);
            }
        }
    }
}