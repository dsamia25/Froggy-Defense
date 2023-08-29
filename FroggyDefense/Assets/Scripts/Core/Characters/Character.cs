using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Items;
using FroggyDefense.UI;
using FroggyDefense.Core.Spells;
using FroggyDefense.Movement;

namespace FroggyDefense.Core
{
    public class Character : MonoBehaviour, IHasStats, IDestructable
    {        
        [Space]
        [Header("Character Info")]
        [Space]
        [SerializeField] private string _name = "NAME HERE";    
        [SerializeField] protected HealthBar m_HealthBar = null;
        [SerializeField] protected HealthBar m_ManaBar = null;
        public ProjectileManager m_ProjectileManager = null;

        public string Name => _name;

        // TODO: Toggle InCombat when in combat.
        [SerializeField] protected bool _inCombat = false;
        public bool InCombat => _inCombat;

        // TODO: Find a place to put this.
        public bool BeingSummoned = false;

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
        [SerializeField] private float _maxXp = 0f;
        [SerializeField] private CharacterLevelExperienceFunction _experienceFunction = null;
        public float Xp { get => _xp; }                                     // The character's experience points.
        public float MaxXp { get => _maxXp; }                         // The amount of xp required to level up.
        public CharacterLevelExperienceFunction ExperienceFunction { get => _experienceFunction; }  // Function used to calculate experience needed to level up.

        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected StatSheet _stats;                        // List of base stats and their growth per level.
        [SerializeField] protected float _health = 1f;
        [SerializeField] protected float _mana = 100;
        public StatSheet Stats => _stats;
        public float MaxHealth => _stats.MaxHealth;
        public float HealthRegen => _stats.HealthRegen * (InCombat ? _stats.CombatHealthRegenModifier : 1);
        public float MaxMana => _stats.MaxMana;
        public float ManaRegen => _stats.ManaRegen * (InCombat ? _stats.CombatManaRegenModifier : 1);
        public float MoveSpeed => _stats.MoveSpeed;
        public float CritChance => _stats.CritChance;
        public float CritBonus => _stats.CritBonus;
        public float DamagedInvincibilityTime => _stats.DamagedInvincibilityTime;

        public float Health
        {
            get => _health;
            set
            {
                float amount = value;
                if (amount > MaxHealth)
                {
                    amount = MaxHealth;
                }
                _health = amount;

                UpdateHealthBar();
            }
        }
        public float Mana {
            get => _mana;
            set
            {
                float amount = value;
                if (amount > MaxMana)
                {
                    amount = MaxMana;
                }
                _mana = amount;

                UpdateManaBar();
            }
        }

        [Space]
        [Header("Status Effects")]
        [Space]
        protected List<AppliedEffect> _appliedEffectList = new List<AppliedEffect>();                                               // List of all status effects applied on the target.
        protected Dictionary<string, AppliedEffect> _appliedEffectIndex = new Dictionary<string, AppliedEffect>();                 // List of all dot names and their applied effects.
        protected bool _isPoisoned = false;
        protected bool _isChilled = false;
        protected bool _isBleeding = false;
        protected bool _isBurning = false;
        protected bool _isRooted = false;
        protected bool _isSlowed = false;
        protected bool _isStunned = false;
        public bool IsPoisoned => _isPoisoned;  // TODO: Implement
        public bool IsChilled => _isChilled;    // TODO: Implement
        public bool IsBleeding => _isBleeding;  // TODO: Implement
        public bool IsBurning => _isBurning;    // TODO: Implement
        public bool IsRooted => _isRooted;      // TODO: Implement
        public bool IsSlowed => _isSlowed;      // TODO: Implement
        public bool IsStunned => _isStunned;

        [SerializeField] protected float _moveSpeedModifer = 1f;            // The percent modifier effecting movement speed.
        [SerializeField] protected int _stunEffectCounter = 0;              // Tracks how many stun effects have been applied. If 0 then is not stunned.
        [SerializeField] protected bool _invincible = false;
        public float MoveSpeedModifer => _moveSpeedModifer;
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        public bool IsDamaged => _health < MaxHealth;

        [Space]
        [Header("Equipment")]
        [Space]
        protected Equipment[] _equipmentSlots;

        protected ObjectController controller;
        protected Vector2 _moveDir = Vector2.zero;

        public delegate void CharacterDelegate();   
        public event CharacterDelegate ExperienceChanged;           // Event when the character has gained experience.
        public event CharacterDelegate LeveledUp;                   // Event when the character levels up.
        public event CharacterDelegate StatsChanged;                // Event triggered when the character's stats have changed.

        // ********************************************************************
        // Stats
        // ********************************************************************
        #region Init
        protected virtual void Awake()
        {
            controller = GetComponent<ObjectController>();

            if (m_ProjectileManager == null) m_ProjectileManager = GetComponent<ProjectileManager>();

            // TODO: Make a way to take in an existing equipment spread.
            InitEquipmentSlots();

            if (_experienceFunction == null)
            {
                _maxXp = 99999999;
            } else
            {
                _maxXp = _experienceFunction.GetMaxXp(_stats.Level);
            }

            if (_stats == null)
            {
                _stats = new StatSheet();
            }
            _stats.SetLevel(1);
        }

        protected virtual void Start()
        {
            _health = MaxHealth;
            if (m_HealthBar != null)
            {
                m_HealthBar.InitBar(MaxHealth);
            }

            _mana = MaxMana;
            if (m_ManaBar != null)
            {
                m_ManaBar.InitBar(MaxMana);
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
            // Regen Health and Mana
            Health += HealthRegen * Time.deltaTime;
            Mana += ManaRegen * Time.deltaTime;

            TickAppliedEffects();
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
            return _stats;
        }

        /// <summary>
        /// Recalculates all the total stat values.
        /// </summary>
        protected void UpdateStats()
        {
            _stats.UpdateStats();
            StatsChanged?.Invoke();
        }

        /// <summary>
        /// Gets the spell power for the type damage.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public float GetSpellPower(DamageType type)
        {
            return _stats.GetSpellPower(type);
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

            //_inventory?.Add(_equipmentSlots[slot], 1);          // Place the item in the inventory if possible.
            
            _equipmentSlots[slot] = null;                       // Clear the equipment slot.
        }

        /// <summary>
        /// Removes the equipment's stats from the player's stats.
        /// </summary>
        protected virtual void RemoveEquipmentStats(Equipment equipment)
        {
            foreach (StatValuePair stat in equipment.Stats)
            {
                _stats.RemoveBonusStat(stat.Stat, stat.Value, false);
            }
            UpdateStats();
        }

        /// <summary>
        /// Adds the equipment's stats to the player's stats.
        /// </summary>
        /// <param name="equipment"></param>
        protected void AddEquipmentStats(Equipment equipment)
        {
            foreach (StatValuePair stat in equipment.Stats)
            {
                _stats.AddBonusStat(stat.Stat, stat.Value, false);
            }
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

        /// <summary>
        /// Uses the character's mana.
        /// </summary>
        /// <returns></returns>
        public void UseMana(float mana)
        {
            Mana -= mana;
            Debug.Log("Using " + mana + ". " + Name + " now has " + _mana + "/" + MaxMana + " mana.");
        }

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
                visualsAnimator?.SetBool("DamagedAnimation", true);
            }

            if (DamagedInvincibilityTime > 0)
            {
                StartCoroutine(DamagedInvincibility());
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
                visualsAnimator?.SetBool("DamagedAnimation", true);
            }

            if (DamagedInvincibilityTime > 0)
            {
                StartCoroutine(DamagedInvincibility());
            }
        }

        /// <summary>
        /// Applies an AppliedEffect such as a slow, stun, or DOT to the target.
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(AppliedEffect effect)
        {
            if (_appliedEffectIndex.ContainsKey(effect.Name))
            {
                _appliedEffectIndex[effect.Name].Refresh();
                Debug.Log("Refreshed status effect [" + effect.Name + "] on [" + gameObject.name + "]. There are now (" + _appliedEffectList.Count + ") status effects.");
                return;
            }
            _appliedEffectList.Add(effect);
            _appliedEffectIndex.Add(effect.Name, effect);

            if (effect.Effect == AppliedEffectType.Stun)
            {
                _stunEffectCounter++;
                _isStunned = true;
            }

            Debug.Log("Applied a new status effect [" + effect.Name + "] to [" + gameObject.name + "]. There are now (" + _appliedEffectList.Count + ") status effects.");
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
            foreach (AppliedEffect status in _appliedEffectList)
            {
                if (status.Effect == AppliedEffectType.Slow)
                {
                    if (Mathf.Abs(status.Strength) > strongestSlow)
                    {
                        strongestSlow = status.Strength;
                    }
                }
            }
            _moveSpeedModifer = (100.0f + strongestSlow) / 100.0f;
            if (_moveSpeedModifer < .1f)
            {
                _moveSpeedModifer = .1f;
            }
            Debug.Log("Calculated Move Speed [" + _moveSpeedModifer + "%] for [" + gameObject.name + "]. There are now (" + _appliedEffectList.Count + ") status effects.");
            controller.MoveSpeedModifier = _moveSpeedModifer;
        }

        /// <summary>
        /// Ticks each of the dots in the list.
        /// </summary>
        public void TickAppliedEffects()
        {
            List<AppliedEffect> expiredEffects = new List<AppliedEffect>();
            foreach (AppliedEffect status in _appliedEffectList)
            {
                status.Tick();

                if (status.IsExpired)
                {
                    expiredEffects.Add(status);
                }
            }

            foreach (AppliedEffect status in expiredEffects)
            {
                RemoveAppliedEffect(status);
            }
        }

        /// <summary>
        /// Removes the dot from the lists.
        /// </summary>
        /// <param name="dot"></param>
        private void RemoveAppliedEffect(AppliedEffect status)
        {
            try
            {
                _appliedEffectIndex.Remove(status.Name);
                _appliedEffectList.Remove(status);

                if (status.Effect == AppliedEffectType.Stun) _stunEffectCounter--;
                if (_stunEffectCounter <= 0) _isStunned = false;

                CalculateMoveSpeedModifer();
                Debug.Log("Removed a new status effect [" + status.Name + "] from [" + gameObject.name + "]. There are now (" + _appliedEffectList.Count + ") status effects.");
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

            // Clear applied effects list.
            for (int i = _appliedEffectList.Count; i >= 0; i--)
            {
                var effect = _appliedEffectList[i];
                effect.Clear();
                _appliedEffectList.Remove(effect);
            }
        }
        #endregion

        private IEnumerator DamagedInvincibility()
        {
            m_Invincible = true;
            yield return new WaitForSeconds(DamagedInvincibilityTime);
            m_Invincible = false;
        }

        /// <summary>
        /// Adds experice to the player and checks if they should level up.
        /// </summary>
        /// <param name="experience"></param>
        public void GainExperience(float experience)
        {
            if (experience < 0) return;

            _xp += experience;

            if (_xp >= _maxXp)
            {
                LevelUp();
            }
            else
            {
                ExperienceChanged?.Invoke();   // Event that the character's experience changed.
            }
        }

        /// <summary>
        /// Sets the character's level.
        /// </summary>
        /// <param name="level"></param>
        public virtual void SetLevel(int level)
        {
            Debug.Log($"Setting {name} level to {level}.");
            _stats.SetLevel(level);
            Debug.Log($"{name} level now {_stats.Level}");
            //LeveledUp?.Invoke();
        }

        /// <summary>
        /// Levels up the character and manages its XP values.
        /// Sends out an event on level up.
        /// </summary>
        public virtual void LevelUp()
        {
            _stats.LevelUp();

            _xp = _xp - _maxXp;
            _maxXp = _experienceFunction.GetMaxXp(_stats.Level);

            if (_xp >= _maxXp) LevelUp();
            LeveledUp?.Invoke();
        }

        /// <summary>
        /// Triggers the summoning visuals. Enemy will override this to set their
        /// behaviour too.
        /// </summary>
        public virtual void SummonAnimation()
        {
            visualsAnimator.SetTrigger("SummonAnimation");
        }

        /// <summary>
        /// Updates the health bar using the current health and max health.
        /// </summary>
        public virtual void UpdateHealthBar()
        {
            if (m_HealthBar != null)
            {
                m_HealthBar.SetMaxHealth(_health, MaxHealth);
            }
        }

        /// <summary>
        /// Updates the mana bar with the current mana and max mana.
        /// </summary>
        public virtual void UpdateManaBar()
        {
            if (m_ManaBar != null)
            {
                m_ManaBar.SetMaxHealth(_mana, MaxMana);
            }
        }
    }
}