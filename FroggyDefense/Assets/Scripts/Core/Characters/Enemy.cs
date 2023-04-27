using System;
using UnityEngine;
using FroggyDefense.Movement;
using FroggyDefense.Interactables;
using FroggyDefense.UI;
using FroggyDefense.Core.Buildings;

namespace FroggyDefense.Core
{
    public class Enemy : MonoBehaviour, IDestructable
    {
        [SerializeField] protected HealthBar m_HealthBar = null;
        [SerializeField] protected bool m_HideHealthBarAtFull = true;

        public int Points = 10;                         // How many points the enemy is worth.
        public int Experience = 10;                     // How much experience the enemy is worth.

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
                if (value <= 0)
                {
                    Die();
                }

                float amount = value;
                if (amount > _maxHealth)
                {
                    amount = _maxHealth;
                }
                _health = amount;

                UpdateHealthBar();
            }
        }
        public bool IsDamaged => _health < _maxHealth;
        [SerializeField] protected float _nexusDamage = 1f;
        public float m_NexusDamage { get => _nexusDamage; set { _nexusDamage = value; } }

        [SerializeField] protected float _damage = 1f;
        public float m_Damage { get => _damage; set { _damage = value; } }


        [SerializeField] protected bool _invincible;
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        [SerializeField] protected bool _splashShield;
        public bool m_SplashShield { get => _splashShield; set { _splashShield = value; } }

        private ObjectController controller;
        private Vector2 moveDir = Vector2.zero;

        public delegate void EnemyDelegate(EnemyEventArgs args);
        public static EnemyDelegate EnemyDamagedEvent;
        public static EnemyDelegate EnemyDefeatedEvent;

        private void Awake()
        {
            controller = GetComponent<ObjectController>();
        }

        private void Start()
        {
            // Init Health bar with max health.
            if (m_HealthBar != null)
            {
                m_HealthBar.InitBar(_maxHealth);

                if (m_HideHealthBarAtFull)
                {
                    m_HealthBar.gameObject.SetActive(IsDamaged);
                }
            }
        }

        private void Update()
        {
            if (GameManager.GameStarted)
            {
                moveDir = FindPath();
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.GameStarted)
            {
                controller.Move(moveDir);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Nexus nexus = null;
            if ((nexus = collision.gameObject.GetComponent<Nexus>()) != null)
            {
                nexus.TakeDamage(m_NexusDamage);
                Kill();
            }

            Player player = null;
            if ((player = collision.gameObject.GetComponent<Player>()) != null)
            {
                player.TakeDamage(m_Damage);
                Kill();
            }
        }

        public void UpdateHealthBar()
        {
            if (m_HealthBar != null)
            {
                m_HealthBar.SetHealth(_health);

                if (m_HideHealthBarAtFull)
                {
                    m_HealthBar.gameObject.SetActive(IsDamaged);
                }
            }
        }

        /// <summary>
        /// Damages the enemy and sends out an event.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            // TODO: Not sure if the event is needed anymore.
            EnemyDamagedEvent?.Invoke(new EnemyEventArgs(transform.position, damage, -1, -1));
            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, damage, NumberPopupType.Damage);
            m_Health -= damage;
        }

        /// <summary>
        /// Starts the enemy's death sequence.
        /// </summary>
        public void Die()
        {
            // TODO: Not sure if the event is needed anymore.
            EnemyDefeatedEvent?.Invoke(new EnemyEventArgs(transform.position, -1, Points, Experience));
            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, Points, NumberPopupType.EnemyDefeated);
            GetComponent<DropGems>().Drop();
            Destroy(gameObject);
        }

        /// <summary>
        /// Instantly kills the enemy without dropping anything or rewarding points.
        /// </summary>
        public void Kill()
        {
            EnemyDefeatedEvent?.Invoke(new EnemyEventArgs(transform.position, -1, -1, -1));
            Destroy(gameObject);
        }

        // TODO: Move to a pathfinding class.
        /// <summary>
        /// Finds a path from the current position to the Nexus.
        /// </summary>
        /// <returns></returns>
        public Vector2 FindPath()
        {
            if (BoardManager.instance.Nexus == null)
            {
                return Vector2.zero;
            }

            Vector2 targetLoc = BoardManager.instance.Nexus.transform.position;
            return (targetLoc - (Vector2)transform.position).normalized;
        }
    }

    public class EnemyEventArgs : EventArgs
    {
        public Vector2 pos;     // The position of the event.
        public float damage;    // How much damage was dealt to the enemy.
        public int points;      // How much points the enemy is worth.
        public int experience;  // How much experience points the enemy is worth.

        public EnemyEventArgs (Vector2 _pos, float _damage, int _points, int _experience)
        {
            pos = _pos;
            damage = _damage;
            points = _points;
            experience = _experience;
        }
    }
}