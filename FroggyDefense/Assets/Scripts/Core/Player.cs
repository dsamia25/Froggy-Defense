using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Movement;
using FroggyDefense.Weapons;
using FroggyDefense.UI;

namespace FroggyDefense.Core
{
    public class Player : Character, IDestructable, IUseWeapon
    {
        [SerializeField] protected HealthBar m_HealthBar = null;

        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected float _maxHealth = 1f;
        [SerializeField] protected float _health = 1f;
        public float m_Health
        {
            get => _health;
            set
            {
                float amount = value;
                if (amount > _maxHealth)
                {
                    amount = _maxHealth;
                }
                _health = amount;

                if (m_HealthBar != null)
                {
                    m_HealthBar.SetMaxHealth(_health, _maxHealth);
                }
            }
        }
        public bool IsDamaged => _health < _maxHealth;
        
        [SerializeField] protected float _moveSpeed = 1f;
        [SerializeField] protected float MoveSpeed => _moveSpeed;   // The player's total attack speed with all buffs applied.

        [SerializeField] protected bool _invincible;
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        [SerializeField] protected bool _splashShield;
        public bool m_SplashShield { get => _splashShield; set { _splashShield = value; } }

        [Header("Animations")]
        public Animator animator = null;
        public Material DamagedMaterial = null;
        public Material DefaultMaterial = null;
        public float m_DamagedAnimationTime = 1f;

        [Space] 
        [Header("Attack Settings")]
        [SerializeField] protected Weapon m_Weapon = null;

        private ObjectController controller;
        private Vector2 _moveDir = Vector2.zero;

        [Space]
        [Header("Player Events")]
        [Space]
        public UnityEvent PlayerDeathEvent;

        private void Awake()
        {
            _health = _maxHealth;
            if (m_HealthBar != null)
            {
                m_HealthBar.SetMaxHealth(_health, _maxHealth);
            }

            controller = GetComponent<ObjectController>();
        }

        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (m_Weapon == null)
            {
                m_Weapon = GetComponent<Weapon>();
            }

            m_HealthBar.traceDelay = m_DamagedAnimationTime;
            Enemy.EnemyDefeatedEvent += OnEnemyDefeatedEvent;
        }

        private void Update()
        {
            if (GameManager.GameStarted)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Shooting");
                    if (GameManager.GameStarted && GameManager.ShootingEnabled)
                    {
                        // TODO: Could move Shooting to an event?
                        // TODO: Change the way the angle is calculated to FroggyDefense.Support.Angles.UnitVectorBetweenTwoPoints()
                        m_Weapon.Shoot((Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(transform.position)).normalized);
                    }
                }

                // Update movement direction.
                _moveDir.x = Input.GetAxisRaw("Horizontal");
                _moveDir.y = Input.GetAxisRaw("Vertical");
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.GameStarted)
            {
                controller.Move(_moveDir);
            }
        }

        /// <summary>
        /// The player takes damage. If they die then invoke their death event.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            if (_invincible) return;

            if (--m_Health <= 0)
            {
                // TODO: Make a saving throw minigame that the player can complete to gain their last health back
                // or die if they fail.
                // TODO: Make a death animation.
                Die();
            }
            else
            {
                animator.SetBool("DamagedInvincibility", true);
            }
        }

        /// <summary>
        /// Resolves the character's death.
        /// </summary>
        public void Die()
        {
            m_Health = 0;
            PlayerDeathEvent?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IGroundInteractable interactable = null;
            if ((interactable = collision.gameObject.GetComponent<IGroundInteractable>()) != null)
            {
                interactable.Interact(gameObject);
            }

            Projectile projectile = null;
            if ((projectile = collision.gameObject.GetComponent<Projectile>()) != null)
            {
                TakeDamage(projectile.m_Damage);
            }

            if (collision.tag == "DeathBox")
            {
                Die();
            }
        }

        // TODO: Make an actual formula for this.
        // TODO: Store this in a value updated in UpdateStats()
        /// <summary>
        /// Returns the player's stats converted into a direct attack value.
        /// </summary>
        /// <returns></returns>
        public float GetDirectDamage()
        {
            return Strength;
        }

        // TODO: Make an actual formula for this.
        // TODO: Store this in a value updated in UpdateStats()
        /// <summary>
        /// Returns the player's stats converted into a splash attack value.
        /// </summary>
        /// <returns></returns>
        public float GetSplashDamage()
        {
            return Intellect;
        }

        /// <summary>
        /// Defeated
        /// </summary>
        /// <param name="args"></param>
        public void OnEnemyDefeatedEvent(EnemyEventArgs args)
        {
            GainExperience(args.experience);    // Gain experience from the defeated enemy.
        }
    }
}