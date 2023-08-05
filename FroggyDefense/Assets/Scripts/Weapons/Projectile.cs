using System;
using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private Character Caster;                                       // Who shot the projectile.
        [SerializeField] private Vector2 m_StorageLoc;                  // Where the projectile is stored.

        //public
        private ProjectileInfo template;
        public ProjectileInfo Template
        {
            get => template;
            private set
            {
                template = value;

                // Change image to projectile image.

            }
        }   // The template to use for info.

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

        /// <summary>
        /// Initializes the Projectile values.
        /// </summary>
        public void Init(ProjectileInfo template)
        {
            Template = template;
        }

        // **************************************************
        // Update
        // **************************************************

        private void FixedUpdate()
        {
            // Only disable after max range if set to.
            if (template.HasMaxRange)
            {
                // Return the projectile after it's traveled out of range.
                if (Vector2.Distance(m_ShootPos, transform.position) >= template.MaxRange)
                {
                    // Include any explosion setting.
                    Explode();
                }
            }

            if (template.HasMaxTime)
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

            if (template.HasSeeking)
            {
                // TODO: Implement some sort of way to track after the target using the m_SeekingTurnAngle.
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        public void Shoot(Character caster, Vector2 dir)
        {
            Caster = caster;

            _pierces = template.MaxPierces;

            m_PrimaryTarget = null;
            m_ShootPos = transform.position;
            m_TimeCounter = template.TimeLimit;

            gameObject.SetActive(true);
            rb.velocity = template.MoveSpeed * dir.normalized;
        }

        /// <summary>
        /// Returns the projectile to its starting position and freezes it.
        /// </summary>
        public void Return()
        {
            rb.velocity = Vector2.zero;
            transform.position = m_StorageLoc;

            ProjectileManager.instance.Return(this);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Causes the projectile to do any of its exploding settings and returns the projectile to its resting location.
        /// Should be called for any situation where the projectile should be destroyed.
        /// </summary>
        public void Explode()
        {
            if (template.HasSplashDamage)
            {
                Collider2D[] targetsHit = Physics2D.OverlapCircleAll(transform.position, template.SplashRadius, (template.SplashLayer == 0 ? gameObject.layer : template.SplashLayer));
                foreach (Collider2D collider in targetsHit)
                {
                    IDestructable destructable = null;
                    if ((destructable = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        if (destructable != m_PrimaryTarget)
                        {
                            destructable.TakeDamage(new DamageAction(Caster, template.SplashDamage + (template.HasSplashDamageScaling ? GetStatScaling(template.SplashDamageScalingFactor) : 0), template.SplashDamageType));
                        }
                    }
                }
            }
            Return();
        }

        /// <summary>
        /// Returns the calculated scaling factor.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetStatScaling(StatValuePair stat)
        {
            if (Caster == null) return 0;
            try
            {
                return stat.Value * Caster.Stats.GetTotalStat(stat.Stat);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error getting stat scaling factor: {e}");
                return 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDestructable destructable = null;
            if ((destructable = collision.gameObject.GetComponent<IDestructable>()) != null)
            {
                destructable.TakeDamage(new DamageAction(Caster, template.Damage + (template.HasProjectileDamageScaling ? GetStatScaling(template.ProjectileDamageScalingFactor) : 0), template.DirectDamageType));
                m_PrimaryTarget = destructable;

                // Explode if it doesn't have piercing or if it's done with piercing.
                if (!template.HasPiercing || (template.HasPiercing && --_pierces < 0))
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
            if (template.HasSplashDamage)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, template.SplashRadius);
            }
        }
    }
}