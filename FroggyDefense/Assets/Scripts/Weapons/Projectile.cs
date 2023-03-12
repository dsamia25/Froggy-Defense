using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Vector2 m_StorageLoc;  // Where the projectile is stored.

        [SerializeField] private float m_MoveSpeed = 1f;    // How fast the projectile moves.

        [Header("Max Range")]
        [Space]
        [SerializeField] private bool m_HasMaxRange = false;    // If the projectile should disappear after the maximum range has been traveled.
        [SerializeField] private float m_MaxRange = 10f;        // How far the projectile should travel before despawning. Default 10 is much longer than the screen.
        private Vector2 m_ShootPos = Vector2.zero;  // Where the projectile was shot from last. Used to calculate distance.

        [Header("Max Time")]
        [Space]
        [SerializeField] private bool m_HasMaxTime = false; // If the projectile should despawn after a certain amount of time.
        [SerializeField] private float m_MaxTime = 5f;  // How long until the projectile blows up on its own.
        private float m_TimeCounter = 0f;   // Counts how long the projectile has been active.

        [Header("Piercing")]
        [Space]
        [SerializeField] private bool m_HasPiercing = false;   // If the projectile pierces through enemies before getting destroyed.
        [SerializeField] private int m_PiercingNumber = 0; // How many targets the projectile pierces through before getting destroyed.

        [Header("Damage")]
        [Space]
        [SerializeField] private float _damage = 1f;
        public float m_Damage { get => _damage; private set => _damage = value; }       // How much damage a direct hit deals.
                
        [Header("Splash Damage")]
        [Space]
        [SerializeField] private bool m_HasSplashDamage = false;    // If the projectile should cast splash damage around it.
        [SerializeField] private float _splashDamage = 0f;          // How much damage the explosion radius has.
        public float m_SplashDamage { get => _splashDamage; private set { _splashDamage = value; } }
        [SerializeField] private LayerMask m_SplashLayer = 0;       // The layer that the explosion hits on.
        [SerializeField] private float m_SplashDamageRadius = 0f;   // How big of a damage radius the projectile has.

        /*
         * TODO: Make a seeking feature.
         * 
         * - On Shoot, select closest enemy target.
         * - In Update, adjust velocity to turn towards the selected target, a larger SeekingTurnAngle will allow for greater maneuverability (Spelling lol).
         * - Seeking targets should have a Max Time always set.
         */
        [Header("Seeking")]
        [Space]
        [SerializeField] private bool m_HasSeeking = false;         // If the projectile should seek after a target.
        [SerializeField] private float m_SeekingTurnAngle = .05f;   // How fast the projectile can adjust to chase its target.

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
                m_StorageLoc = transform.position;
            }
        }

        // **************************************************
        // Update
        // **************************************************

        private void FixedUpdate()
        {
            // Only disable after max range if set to.
            if (m_HasMaxRange)
            {
                // Return the projectile after it's traveled out of range.
                if (Vector2.Distance(m_ShootPos, transform.position) >= m_MaxRange)
                {
                    // Include any explosion setting.
                    Explode();
                }
            }

            if (m_HasMaxTime)
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

            if (m_HasSeeking)
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
        public void Shoot(Vector2 dir, float directDamage, float splashDamage)
        {
            m_Damage = directDamage;
            m_SplashDamage = splashDamage;

            m_PrimaryTarget = null;
            m_ShootPos = transform.position;
            m_TimeCounter = m_MaxTime;

            gameObject.SetActive(true);
            rb.velocity = m_MoveSpeed * dir.normalized;
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
            if (m_HasSplashDamage)
            {
                Collider2D[] targetsHit = Physics2D.OverlapCircleAll(transform.position, m_SplashDamageRadius, (m_SplashLayer == 0 ? gameObject.layer : m_SplashLayer));
                foreach (Collider2D collider in targetsHit)
                {
                    IDestructable destructable = null;
                    if ((destructable = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        if (destructable != m_PrimaryTarget)
                        {
                            destructable.TakeDamage(m_SplashDamage);
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
                destructable.TakeDamage(m_Damage);
                m_PrimaryTarget = destructable;

                // Explode if it doesn't have piercing or if it's done with piercing.
                if (!m_HasPiercing || (m_HasPiercing && --m_PiercingNumber <= 0))
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
            if (m_HasSplashDamage)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, m_SplashDamageRadius);
            }
        }
    }
}