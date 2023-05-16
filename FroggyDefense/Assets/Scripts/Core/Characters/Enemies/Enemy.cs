using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Movement;
using FroggyDefense.Interactables;
using FroggyDefense.UI;
using FroggyDefense.Core.Buildings;
using FroggyDefense.Core.Spells;
using FroggyDefense.Weapons;

namespace FroggyDefense.Core.Enemies
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
        [SerializeField] protected float _nexusDamage = 1f;
        [SerializeField] protected float _damage = 1f;
        [SerializeField] protected float _moveSpeedModifer = 1f;
        [SerializeField] protected int _stunEffectCounter = 0;              // Tracks how many stun effects have been applied. If 0 then is not stunned.
        [SerializeField] protected bool _isStunned = false;
        [SerializeField] protected bool _invincible = false;
        [SerializeField] protected bool _splashShield = false;
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
        public float m_NexusDamage { get => _nexusDamage; set { _nexusDamage = value; } }
        public float m_Damage { get => _damage; set { _damage = value; } }
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        public bool m_SplashShield { get => _splashShield; set { _splashShield = value; } }
        public float MoveSpeedModifier => _moveSpeedModifer;
        public bool IsStunned => _isStunned;

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

        [Space]
        [Header("Status Effects")]
        [Space] 
        public List<StatusEffect> _statusEffects = new List<StatusEffect>();                                                                // List of all status effects applied on the target.
        [SerializeField] private Dictionary<string, StatusEffect> _appliedEffects = new Dictionary<string, StatusEffect>();                 // List of all dot names and their applied effects.
        public List<DamageOverTimeEffect> _dots = new List<DamageOverTimeEffect>();                                                         // List of all dots applied on the target.
        [SerializeField] private Dictionary<string, DamageOverTimeEffect> _appliedDots = new Dictionary<string, DamageOverTimeEffect>();    // List of all dot names and their applied effects.

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
                moveDir = FindPath(_focus);

                TickDots();
                TickStatusEffects();
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
                    controller.Move(moveDir);
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

        #region IDestructable
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

        // TODO: Make this be effected by resistances to the type of damage and stuff.
        /// <summary>
        /// Applies a damage action to the target.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(DamageAction damage)
        {
            EnemyDamagedEvent?.Invoke(new EnemyEventArgs(transform.position, damage.Damage, -1, -1));
            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, damage.Damage, NumberPopupType.Damage);
            m_Health -= damage.Damage;
        }

        // TODO: Merge ApplyDot and ApplyStatusEffect into the same method.
        /// <summary>
        /// Applies an overtime effect to the thing.
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyDot(DamageOverTimeEffect dot)
        {
            if (_appliedDots.ContainsKey(dot.Name))
            {
                _appliedDots[dot.Name].Refresh();
                return;
            }
            _dots.Add(dot);
            _appliedDots.Add(dot.Name, dot);
        }

        /// <summary>
        /// Applies a status effect.
        /// </summary>
        /// <param name="status"></param>
        public void ApplyStatusEffect(StatusEffect status)
        {
            if (_appliedEffects.ContainsKey(status.Name))
            {
                _appliedEffects[status.Name].Refresh();
                Debug.Log("Refreshed status effect [" + status.Name + "] on [" + gameObject.name + "]. There are now (" + _statusEffects.Count + ") status effects.");
                return;
            }
            _statusEffects.Add(status);
            _appliedEffects.Add(status.Name, status);

            if (status.StatusType == StatusEffectType.Stun)
            {
                _stunEffectCounter++;
                _isStunned = true;
            }

            Debug.Log("Applied a new status effect [" + status.Name + "] to [" + gameObject.name + "]. There are now (" + _statusEffects.Count + ") status effects.");
            CalculateMoveSpeedModifer();
        }

        public void KnockBack(Vector2 dir, float strength, float knockBackTime, float moveLockTime)
        {
            controller.Lunge(dir, strength, knockBackTime, moveLockTime);
        }

        /// <summary>
        /// Finds the new strongest slow and stun effects.
        /// </summary>
        private void CalculateMoveSpeedModifer()
        {
            if (_isStunned)
            {
                Debug.Log("Calculated Move Speed [" + _moveSpeedModifer + "%] for [" + gameObject.name + "]. Is Stunned.");
                _moveSpeedModifer = 1f;
                return;
            }

            float strongestSlow = 0f;
            foreach (StatusEffect status in _statusEffects)
            {
                if (status.StatusType == StatusEffectType.Slow)
                {
                    if (Mathf.Abs(status.EffectStrength) > strongestSlow)
                    {
                        strongestSlow = status.EffectStrength;
                    }
                }
            }
            _moveSpeedModifer = (100.0f + strongestSlow) / 100.0f;
            if (_moveSpeedModifer < .1f)
            {
                _moveSpeedModifer = .1f;
            }
            Debug.Log("Calculated Move Speed [" + _moveSpeedModifer + "%] for [" + gameObject.name + "]. There are now (" + _statusEffects.Count + ") status effects.");
            controller.MoveSpeedModifier = _moveSpeedModifer;
        }

        /// <summary>
        /// Ticks each of the dots in the list.
        /// </summary>
        public void TickDots()
        {
            List<DamageOverTimeEffect> expiredEffects = new List<DamageOverTimeEffect>();
            foreach (DamageOverTimeEffect dot in _dots)
            {
                dot.Tick();

                if (dot.TicksLeft <= 0)
                {
                    expiredEffects.Add(dot);
                }
            }

            foreach (DamageOverTimeEffect dot in expiredEffects)
            {
                RemoveDot(dot);
            }
        }

        /// <summary>
        /// Ticks each of the dots in the list.
        /// </summary>
        public void TickStatusEffects()
        {
            List<StatusEffect> expiredEffects = new List<StatusEffect>();
            foreach (StatusEffect status in _statusEffects)
            {
                status.Tick();

                if (status.TimeLeft <= 0)
                {
                    expiredEffects.Add(status);
                }
            }

            foreach (StatusEffect status in expiredEffects)
            {
                RemoveStatusEffect(status);
            }
        }

        /// <summary>
        /// Removes the dot from the lists.
        /// </summary>
        /// <param name="dot"></param>
        private void RemoveDot(DamageOverTimeEffect dot)
        {
            try
            {
                _appliedDots.Remove(dot.Name);
                _dots.Remove(dot);
            } catch
            {
                Debug.LogWarning("Aborting removing DOT (" + dot.Name + ").");
            }
        }

        /// <summary>
        /// Removes the dot from the lists.
        /// </summary>
        /// <param name="dot"></param>
        private void RemoveStatusEffect(StatusEffect status)
        {
            try
            {
                _appliedEffects.Remove(status.Name);
                _statusEffects.Remove(status);

                if (status.StatusType == StatusEffectType.Stun) _stunEffectCounter--;
                if (_stunEffectCounter <= 0) _isStunned = false;

                CalculateMoveSpeedModifer();
                Debug.Log("Removed a new status effect [" + status.Name + "] from [" + gameObject.name + "]. There are now (" + _statusEffects.Count + ") status effects.");
            }
            catch
            {
                Debug.LogWarning("Aborting removing DOT (" + status.Name + ").");
            }
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
        #endregion

        public virtual void Attack()
        {

        }

        /// <summary>
        /// Instantly kills the enemy without dropping anything or rewarding points.
        /// </summary>
        public void Kill()
        {
            EnemyDefeatedEvent?.Invoke(new EnemyEventArgs(transform.position, -1, -1, -1));
            Destroy(gameObject);
        }

        /// <summary>
        /// Resets the enemy's focus to the nexus.
        /// </summary>
        public void ResetFocus()
        {
            if (BoardManager.instance.Nexus == null)
            {
                Debug.LogWarning("Cannot find Nexus.");
            }
            _focus = BoardManager.instance.Nexus;
        }

        // TODO: Utilize pathfinding class.
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
            Vector2 targetLoc = focus.transform.position;

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