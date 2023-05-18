using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core.Items;
using FroggyDefense.UI;
using FroggyDefense.Core.Spells;
using FroggyDefense.Movement;
using FroggyDefense.Economy;

namespace FroggyDefense.Core
{
    public abstract class Character : MonoBehaviour, IHasStats, IDestructable
    {
        [Space]
        [Header("Character Info")]
        [Space]
        [SerializeField] private string _name = "NAME HERE";    
        public string Name { get => _name; }                    // The character's name.
        [SerializeField] protected HealthBar m_HealthBar = null;

        [Space]
        [Header("Animations")]
        [Space]
        public Animator visualsAnimator = null;
        public Material DamagedMaterial = null;
        public Material DefaultMaterial = null;
        public float m_DamagedAnimationTime = 1f;

        [Space]
        [Header("Experience")]
        [Space]
        [SerializeField] private float _xp = 0f;
        [SerializeField] private float _xpNeeded = 0f;
        [SerializeField] private CharacterLevelExperienceFunction _experienceFunction = null;
        public float Xp { get => _xp; }                                     // The character's experience points.
        public float XpNeeded { get => _xpNeeded; }                         // The amount of xp required to level up.
        public CharacterLevelExperienceFunction ExperienceFunction { get => _experienceFunction; }  // Function used to calculate experience needed to level up.

        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected float _maxHealth = 1f;
        [SerializeField] protected float _health = 1f;
        [SerializeField] protected float _maxMana = 100;
        [SerializeField] protected float _mana = 100;
        [SerializeField] protected float _moveSpeed = 1f;
        [SerializeField] protected StatSheet _stats;                        // List of base stats and their growth per level.
        public float MaxHealth { get => _maxHealth; }
        public float Health
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

                UpdateHealthBar();
            }
        }
        public float MaxMana { get => _maxMana; }
        public float Mana { get => _mana; }
        public float MoveSpeed => _moveSpeed;   // The player's total attack speed with all buffs applied.
        public StatSheet Stats => _stats;

        [Space]
        [Header("Status Effects")]
        [Space]
        [SerializeField] protected bool _isStunned = false;                 // True if currently stunned.
        [SerializeField] protected float _moveSpeedModifer = 1f;            // The percent modifier effecting movement speed.
        [SerializeField] protected int _stunEffectCounter = 0;              // Tracks how many stun effects have been applied. If 0 then is not stunned.
        [SerializeField] protected bool _invincible = false;
        [SerializeField] protected bool _splashShield = false;
        public bool IsStunned => _isStunned;
        public float MoveSpeedModifer => _moveSpeedModifer;
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        public bool m_SplashShield { get => _splashShield; set { _splashShield = value; } }
        public bool IsDamaged => _health < _maxHealth;

        public List<StatusEffect> _statusEffects = new List<StatusEffect>();                                                                // List of all status effects applied on the target.
        private Dictionary<string, StatusEffect> _appliedEffects = new Dictionary<string, StatusEffect>();                 // List of all dot names and their applied effects.
        public List<DamageOverTimeEffect> _dots = new List<DamageOverTimeEffect>();                                                         // List of all dots applied on the target.
        private Dictionary<string, DamageOverTimeEffect> _appliedDots = new Dictionary<string, DamageOverTimeEffect>();    // List of all dot names and their applied effects.

        [Space]
        [Header("Equipment")]
        [Space]
        private Equipment[] _equipmentSlots;

        [Space]
        [Header("Inventory")]
        [Space]
        [SerializeField] private Inventory _inventory;
        public Inventory CharacterInventory { get => _inventory; }

        [Space]
        [Header("Wallet")]
        [SerializeField] private CurrencyWallet _wallet;
        public CurrencyWallet CharacterWallet { get => _wallet; }

        [Space]
        [Header("Character Events")]
        [Space]
        public UnityEvent CharacterStatsChanged;        // TODO: Change this to a normal C# event like the CharacterExperienceChanged event below.

        protected ObjectController controller;
        protected Vector2 _moveDir = Vector2.zero;

        public delegate void CharacterDelegate();
        public event CharacterDelegate CharacterExperienceChanged;
        public event CharacterDelegate CharacterLeveledUp;                  // Event when the character levels up.

        // ********************************************************************
        // Stats
        // ********************************************************************
        #region Init
        protected virtual void Awake()
        {
            controller = GetComponent<ObjectController>();

            if (_inventory == null) _inventory = gameObject.GetComponent<Inventory>();

            // TODO: Make a way to take in an existing equipment spread.
            InitEquipmentSlots();

            if (_experienceFunction == null)
            {
                _xpNeeded = 99999999;
            } else
            {
                _xpNeeded = _experienceFunction.GetXpNeeded(_stats.Level);
            }
            _stats.SetLevel(1);
        }

        protected virtual void Start()
        {
            _health = _maxHealth;
            if (m_HealthBar != null)
            {
                m_HealthBar.InitBar(_maxHealth);
            }
        }

        private void InitEquipmentSlots()
        {
            int slotAmount = Enum.GetValues(typeof(EquipmentSlot)).Length;
            _equipmentSlots = new Equipment[slotAmount];
        }
        #endregion

        // ********************************************************************
        // Update
        // ********************************************************************
        #region Update
        protected virtual void Update()
        {
            TickDots();
            TickStatusEffects();
        }
        #endregion

        // ********************************************************************
        // Stats
        // ********************************************************************
        #region Stats
        /// <summary>
        /// Gets the unit's stat sheet.
        /// </summary>
        /// <returns></returns>
        public StatSheet GetStats()
        {
            return null;
        }

        /// <summary>
        /// Recalculates all the total stat values.
        /// </summary>
        protected void UpdateStats()
        {
            _stats.UpdateStats();
            CharacterStatsChanged?.Invoke();
        }
        #endregion

        // ********************************************************************
        // Equipment
        // ********************************************************************
        #region Equipment
        /// <summary>
        /// Equips the item to its slot and adds its stats to the player's total stat value.
        /// </summary>
        /// <param name="equipment"></param>
        public virtual void Equip(Equipment equipment)
        {
            if (_equipmentSlots == null) InitEquipmentSlots();
            int slot = (int)equipment.Slot;
            Debug.Log("Equipping slot " + equipment.Slot.ToString() + " (" + slot + ").");
            if (_equipmentSlots[slot] != null)
            {
                Unequip(slot);
            }
            _equipmentSlots[slot] = equipment;
            AddEquipmentStats(equipment);
        }

        /// <summary>
        /// Removes the equipment at the given slot and subtracts its stats from the character.
        /// </summary>
        /// <param name="slot"></param>
        public virtual void Unequip(EquipmentSlot slot)
        {
            Unequip((int)slot);
        }

        /// <summary>
        /// Removes the equipment at the given slot and subtracts its stats from the character.
        /// </summary>
        /// <param name="slot"></param>
        public virtual void Unequip(int slot)
        {
            if (_equipmentSlots == null) InitEquipmentSlots();
            Debug.Log("Unequipping slot (" + slot + ").");

            if (_equipmentSlots[slot] == null) return;
            RemoveEquipmentStats(_equipmentSlots[slot]);

            _inventory?.Add(_equipmentSlots[slot], 1);          // Place the item in the inventory if possible.
            
            _equipmentSlots[slot] = null;                       // Clear the equipment slot.
        }

        // TODO: Make the stat increases in equipment a list that can be dynamically sorted through instead of hard-coding it like this.
        /// <summary>
        /// Removes the equipment's stats from the player's stats.
        /// </summary>
        protected virtual void RemoveEquipmentStats(Equipment equipment)
        {
            _stats.RemoveBonusStat(StatType.Strength, equipment.Strength, false);
            _stats.RemoveBonusStat(StatType.Dexterity, equipment.Dexterity, false);
            _stats.RemoveBonusStat(StatType.Agility, equipment.Agility, false);
            _stats.RemoveBonusStat(StatType.Intellect, equipment.Intellect, false);
            _stats.RemoveBonusStat(StatType.Spirit, equipment.Spirit, false);
            UpdateStats();
        }

        /// <summary>
        /// Adds the equipment's stats to the player's stats.
        /// </summary>
        /// <param name="equipment"></param>
        protected void AddEquipmentStats(Equipment equipment)
        {
            _stats.AddBonusStat(StatType.Strength, equipment.Strength, false);
            _stats.AddBonusStat(StatType.Dexterity, equipment.Dexterity, false);
            _stats.AddBonusStat(StatType.Agility, equipment.Agility, false);
            _stats.AddBonusStat(StatType.Intellect, equipment.Intellect, false);
            _stats.AddBonusStat(StatType.Spirit, equipment.Spirit, false);
            UpdateStats();
        }

        /// <summary>
        /// Gets the equipment at the given slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Equipment GetEquipment(int slot)
        {
            if (slot < 0 || slot >= _equipmentSlots.Length) return null;

            return _equipmentSlots[slot];
        }
        #endregion

        // ********************************************************************
        // IDestructable
        // ********************************************************************
        #region IDestructable
        /// <summary>
        /// Damages the enemy and sends out an event.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            if (_invincible) return;

            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, damage, NumberPopupType.Damage);
            Health -= damage;

            if (Health <= 0)
            {
                Die();
            }
            else
            {
                visualsAnimator?.SetBool("DamagedInvincibility", true);
            }
        }

        // TODO: Make this be effected by resistances to the type of damage and stuff.
        /// <summary>
        /// Applies a damage action to the target.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(DamageAction damage)
        {
            if (_invincible) return;

            GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, damage.Damage, NumberPopupType.Damage);
            Health -= damage.Damage;

            if (Health <= 0)
            {
                Die();
            }
            else
            {
                visualsAnimator?.SetBool("DamagedInvincibility", true);
            }
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

        /// <summary>
        /// Knocks back the character and locks them out of movement temporarily.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="strength"></param>
        /// <param name="knockBackTime"></param>
        /// <param name="moveLockTime"></param>
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
            }
            catch
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
        public virtual void Die()
        {
            Debug.Log("Character " + Name + " died.");
        }
        #endregion

        /// <summary>
        /// Adds experice to the player and checks if they should level up.
        /// </summary>
        /// <param name="experience"></param>
        public void GainExperience(float experience)
        {
            if (experience < 0) return;

            _xp += experience;

            if (_xp >= _xpNeeded)
            {
                LevelUp();
            }
            else
            {
                CharacterExperienceChanged?.Invoke();   // Event that the character's experience changed.
            }
        }

        /// <summary>
        /// Levels up the character and manages its XP values.
        /// Sends out an event on level up.
        /// </summary>
        public virtual void LevelUp()
        {
            _stats.LevelUp();

            _xp = _xp - _xpNeeded;
            _xpNeeded = _experienceFunction.GetXpNeeded(_stats.Level);

            if (_xp >= _xpNeeded) LevelUp();
            CharacterLeveledUp?.Invoke();
        }

        public virtual void UpdateHealthBar()
        {
            if (m_HealthBar != null)
            {
                m_HealthBar.SetMaxHealth(_health, _maxHealth);
            }
        }
    }
}