using System;
using UnityEngine;
using FroggyDefense.Core.Buildings;
using FroggyDefense.Weapons;
using FroggyDefense.Interactables;
using Pathfinder;

namespace FroggyDefense.Core.Enemies
{
    public class Enemy : Character, IDestructable
    {
        [SerializeField] protected bool m_HideHealthBarAtFull = true;

        public int Points = 10;                                             // How many points the enemy is worth.
        public int Experience = 10;                                         // How much experience the enemy is worth.

        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected float _nexusDamage = 1f;
        [SerializeField] protected float _damage = 1f;
        public float m_NexusDamage { get => _nexusDamage; set { _nexusDamage = value; } }
        public float m_Damage { get => _damage; set { _damage = value; } }

        [Space]
        [Header("Attacking")]
        [Space]
        [SerializeField] private WeaponUser _weapon;
        [SerializeField] private float _attackRange;
        public float AttackRange = 1f;
    
        [Space]
        [Header("Targetting")]
        [Space]
        [SerializeField] private bool _targetsPlayers;                      // If the enemy will target the player.
        [SerializeField] protected LayerMask m_TargetLayer = 0;             // Which layer in which the enemy looks for targets.
        [SerializeField] protected float _targetDetectionRadius = 1f;       // How far away the enemy will detect the player.
        [SerializeField] protected float _targetCheckFrequency = .1f;       // How often the turret checks for new targets.
        [SerializeField] protected float _leashRadius = 1f;                 // How far away the player has to be to break the leash.
        [SerializeField] protected float _leashBreakTime = 1f;              // How long the player can be outside the leash radius for the leash to break.
        [SerializeField] protected bool _hasMaxLeashTime = false;           // If there is a time limit to break the leash.
        [SerializeField] protected float _maxleashTime = 30f;               // If there is a time limit until the leash manually breaks.
        [SerializeField] protected float _leashResetTime = 5f;              // How long from the leash breaking until it can be leashed again.
        [SerializeField] protected GameObject _focus = null;                // The thing this will chase and attack.
        public bool TargetsPlayer => _targetsPlayers;              
        public float TargetDetectionRadius => _targetDetectionRadius;
        public float TargetCheckFrequency => _targetCheckFrequency;
        public float LeashRadius => _leashRadius;
        public float LeashBreakTime => _leashBreakTime;
        public bool HasMaxLeashTime => _hasMaxLeashTime;
        public float MaxLeashTime => _maxleashTime;
        public float LeashResetTime => _leashResetTime;
        public GameObject Focus { get => _focus; set { _focus = value; } }

        public SpawnZone spawner { get; set; }                              // What spawned in this enemy.

        [Space]
        [Header("Pathing")]
        [Space]
        [SerializeField] private UnitPathfinder pathfinder;
        public LayerInfo WalkableTiles = new LayerInfo();
        public float ResetPathFrequency = 1f;                   // How often the enemy checks for a new path.
        private float _resetPathFrequencyTimer = 0f;

        public delegate void EnemyDelegate(EnemyEventArgs args);
        public static EnemyDelegate EnemyDamagedEvent;
        public static EnemyDelegate EnemyDefeatedEvent;

        protected override void Start()
        {
            base.Start();

            // Init Health bar with max health.
            if (m_HealthBar != null)
            {
                if (m_HideHealthBarAtFull)
                {
                    m_HealthBar.gameObject.SetActive(IsDamaged);
                }
            }

            ResetFocus();
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.GameStarted)
            {
                _moveDir = FindPath(_focus);

                if (_resetPathFrequencyTimer <= 0f)
                {
                    pathfinder.SetPath();
                    _resetPathFrequencyTimer = ResetPathFrequency;
                } else
                {
                    _resetPathFrequencyTimer -= Time.deltaTime;
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.GameStarted)
            {
                if (_isStunned)
                {
                    controller.Freeze();
                } else
                {
                    controller.Move(_moveDir);
                }
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

        public override void UpdateHealthBar()
        {
            base.UpdateHealthBar();

            if (m_HealthBar != null)
            {
                if (m_HideHealthBarAtFull)
                {
                    m_HealthBar.gameObject.SetActive(IsDamaged);
                }
            }
        }

        #region IDestructable
        public override void Die()
        {
            EnemyDefeatedEvent?.Invoke(new EnemyEventArgs(this, transform.position, -1, Points, Experience));
            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, Points, NumberPopupType.EnemyDefeated);
            GetComponent<DropGems>().Drop();
            GetComponent<DropItems>().Drop();
            Destroy(gameObject);
        }
        #endregion

        /// <summary>
        /// Instantly kills the enemy without dropping anything or rewarding points.
        /// </summary>
        public void Kill()
        {
            EnemyDefeatedEvent?.Invoke(new EnemyEventArgs(this, transform.position, -1, -1, -1));
            Destroy(gameObject);
        }

        /// <summary>
        /// Resets the enemy's focus to the nexus.
        /// </summary>
        public void ResetFocus()
        {
            //if (BoardManager.instance.Nexus == null)
            //{
            //    Debug.LogWarning("Cannot find Nexus.");
            //}
            //_focus = BoardManager.instance.Nexus;
            if (spawner != null)
            {
                _focus = spawner.gameObject;
            } else
            {
                _focus = null;
            }
        }

        /// <summary>
        /// Finds a path from the current position to the focus.
        /// </summary>
        /// <returns></returns>
        public Vector2 FindPath(GameObject focus)
        {
            if (focus == null)
            {
                return Vector2.zero;
            }

            // TODO: Currently just draws a straight line to the focus, make a pathfinding class to direct the enemy on an actual path.
            //Vector2 targetLoc = focus.transform.position;
            Vector2 targetLoc = pathfinder.FindPath();

            return (targetLoc - (Vector2)transform.position).normalized;
        }

        /// <summary>
        /// Looks for nearby players to target.
        /// </summary>
        public Collider2D DetectTargets()
        {
            Collider2D[] targets = GetTargets();
            if (targets.Length <= 0)
            {
                return null;
            }

            // TODO: Make a threat system to prioritize certain targets.
            // Finds the closest enemy.
            Collider2D focus = targets[0];
            float shortestDistance = Vector2.Distance(transform.position, targets[0].transform.position);
            foreach (Collider2D target in targets)
            {
                float temp = Vector2.Distance(transform.position, target.transform.position);
                if (temp < shortestDistance)
                {
                    shortestDistance = temp;
                    focus = target;
                }
            }

            return focus;
        }

        /// <summary>
        /// Returns all targets the turret can see.
        /// </summary>
        /// <returns></returns>
        public Collider2D[] GetTargets()
        {
            return Physics2D.OverlapCircleAll(transform.position, TargetDetectionRadius, (m_TargetLayer == 0 ? gameObject.layer : m_TargetLayer));
        }
    }

    // TODO: Make a more generic system event that has an enum for event type like "Enemy Defeated", "Damage Action", "Item Pickup", "Currency Pickup", etc...
    public class EnemyEventArgs : EventArgs
    {
        public Enemy enemy;     // Who died.
        public Vector2 pos;     // The position of the event.
        public float damage;    // How much damage was dealt to the enemy.
        public int points;      // How much points the enemy is worth.
        public int experience;  // How much experience points the enemy is worth.

        public EnemyEventArgs (Enemy _enemy, Vector2 _pos, float _damage, int _points, int _experience)
        {
            enemy = _enemy;
            pos = _pos;
            damage = _damage;
            points = _points;
            experience = _experience;
        }
    }
}