using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private Weapon _weapon;                                         // The weapon that fired this projectile.
        [SerializeField] private Vector2 m_StorageLoc;                  // Where the projectile is stored.

        private Vector2 m_ShootPos = Vector2.zero;                      // Where the projectile was shot from last. Used to calculate distance.
        private float m_TimeCounter = 0f;                               // Counts how long the projectile has been active.
        private int _pierces = 0;                                       // How many targets this projectile has pierced this launch.

        private float _directDamage = 0;                                // How much damage the projectile will do on a direct hit.
        private float _splashDamage = 0;                                // How much damage the projectile will do as splash damage.
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
            if (_weapon.m_HasMaxRange)
            {
                // Return the projectile after it's traveled out of range.
                if (Vector2.Distance(m_ShootPos, transform.position) >= _weapon.m_MaxRange)
                {
                    // Include any explosion setting.
                    Explode();
                }
            }

            if (_weapon.m_HasMaxTime)
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

            if (_weapon.m_HasSeeking)
            {
                // TODO: Implement some sort of way to track after the target using the m_SeekingTurnAngle.
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        /// <summary>
        /// Shoots the projectile at it's travel speed in the given direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Shoot(Weapon weapon, Vector2 dir, float directDamage, float splashDamage)
        {
            _weapon = weapon;
            _directDamage = directDamage;
            _splashDamage = splashDamage;
            _pierces = _weapon.m_PiercingNumber;

            m_PrimaryTarget = null;
            m_ShootPos = transform.position;
            m_TimeCounter = _weapon.m_TimeLimit;

            gameObject.SetActive(true);
            rb.velocity = _weapon.m_ProjectileSpeed * dir.normalized;
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
            if (_weapon.m_HasSplashDamage)
            {
                Collider2D[] targetsHit = Physics2D.OverlapCircleAll(transform.position, _weapon.m_SplashRadius, (_weapon.m_SplashLayer == 0 ? gameObject.layer : _weapon.m_SplashLayer));
                foreach (Collider2D collider in targetsHit)
                {
                    IDestructable destructable = null;
                    if ((destructable = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        if (destructable != m_PrimaryTarget)
                        {
                            destructable.TakeDamage(_splashDamage);
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
                destructable.TakeDamage(_directDamage);
                m_PrimaryTarget = destructable;

                // Explode if it doesn't have piercing or if it's done with piercing.
                if (!_weapon.m_HasPiercing || (_weapon.m_HasPiercing && --_pierces < 0))
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
            if (_weapon.m_HasSplashDamage)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _weapon.m_SplashRadius);
            }
        }
    }
}