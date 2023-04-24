using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core.Items;
using FroggyDefense.Economy;

namespace FroggyDefense.Core
{
    public enum StatType
    {
        NULL,
        strength,
        dexterity,
        agility,
        intellect,
        spirit
    }

    public class Character : MonoBehaviour
    {
        [Space]
        [Header("Character Info")]
        [Space]
        [SerializeField] private string _name = "NAME HERE";    
        public string Name { get => _name; }                    // The character's name.

        [Space]
        [Header("Level")]
        [Space]
        [SerializeField] private int _level = 1;
        [SerializeField] private float _xp = 0f;
        [SerializeField] private float _xpNeeded = 0f;
        [SerializeField] private CharacterLevelExperienceFunction _experienceFunction = null;
        public int Level { get => _level; }                     // The character's level.
        public float Xp { get => _xp; }                         // The character's experience points.
        public float XpNeeded { get => _xpNeeded; }             // The amount of xp required to level up.
        public CharacterLevelExperienceFunction ExperienceFunction { get => _experienceFunction; }  // Function used to calculate experience needed to level up.

        [Space]
        [Header("Base Stats")]
        [Space]
        [SerializeField] private float _baseStrength = 1f;      // Base stats.
        [SerializeField] private float _baseDexterity = 1f;     // Base stats.
        [SerializeField] private float _baseAgility = 1f;       // Base stats.
        [SerializeField] private float _baseIntellect = 1f;     // Base stats.
        [SerializeField] private float _baseSpirit = 1f;        // Base stats.

        [Space]
        [Header("Base Stat Growth")]
        [Space]
        [SerializeField] private float _strengthGrowth = 1f;       // How much of this stat is added on leveling up.
        [SerializeField] private float _dexterityGrowth = 1f;      // How much of this stat is added on leveling up.
        [SerializeField] private float _agilityGrowth = 1f;        // How much of this stat is added on leveling up.
        [SerializeField] private float _intellectGrowth = 1f;      // How much of this stat is added on leveling up.
        [SerializeField] private float _spiritGrowth = 1f;         // How much of this stat is added on leveling up.

        [Space]
        [Header("Stat Increases")]
        [Space]
        [SerializeField] private float _strengthIncrease = 0f;  // Flat increase to increase total stats.
        [SerializeField] private float _dexterityIncrease = 0f; // Flat increase to increase total stats.
        [SerializeField] private float _agilityIncrease = 0f;   // Flat increase to increase total stats.
        [SerializeField] private float _intellectIncrease = 0f; // Flat increase to increase total stats.
        [SerializeField] private float _spiritIncrease = 0f;    // Flat increase to increase total stats.

        [Space]
        [Header("Stat Modifiers")]
        [Space]
        [SerializeField] private float _strengthMod = 1f;
        [SerializeField] private float _dexterityMod = 1f;
        [SerializeField] private float _agilityMod = 1f;
        [SerializeField] private float _intellectMod = 1f;
        [SerializeField] private float _spiritMod = 1f;
        public float StrengthMod { get => _strengthMod; set { _strengthMod = value; } }     // Percent modifier to increase total stats by.
        public float DexterityMod { get => _dexterityMod; set { _dexterityMod = value; } }  // Percent modifier to increase total stats by.
        public float AgilityMod { get => _agilityMod; set { _agilityMod = value; } }        // Percent modifier to increase total stats by.
        public float IntellectMod { get => _intellectMod; set { _intellectMod = value; } }  // Percent modifier to increase total stats by.
        public float SpiritMod { get => _spiritMod; set { _spiritMod = value; } }           // Percent modifier to increase total stats by.

        [Space]
        [Header("Total Stats")]
        [Space]
        [SerializeField] private float _strength = 1f;
        [SerializeField] private float _dexterity = 1f;
        [SerializeField] private float _agility = 1f;
        [SerializeField] private float _intellect = 1f;
        [SerializeField] private float _spirit = 1f;

        public float Strength => _strength;        // Total stat to be used in calculations.
        public float Dexterity => _dexterity;    // Total stat to be used in calculations.
        public float Agility => _agility;            // Total stat to be used in calculations.
        public float Intellect => _intellect;    // Total stat to be used in calculations.
        public float Spirit => _spirit;                // Total stat to be used in calculations.

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

        // TODO: Check if need these.
        // TODO: Probably remove these and the classes entirely.
        [Space]
        [Header("Buffs")]
        [Space]
        public List<Buff> Buffs = new List<Buff>();                         // List of all buffs with references to their effects.
        public List<ValueBuff> ValueBuffs = new List<ValueBuff>();          // List of all value buffs.
        public List<PercentBuff> PercentBuffs = new List<PercentBuff>();    // List of all percent buffs.

        [Space]
        [Header("Character Events")]
        [Space]
        public UnityEvent CharacterStatsChanged;        // TODO: Change this to a normal C# event like the CharacterExperienceChanged event below.

        public delegate void CharacterDelegate();
        public event CharacterDelegate CharacterExperienceChanged;          // TODO: Make an experience bar UI system using this.
        public event CharacterDelegate CharacterLeveledUp;                  // Event when the character levels up.

        private void Awake()
        {
            // TODO: Make a way to take in an existing equipment spread.
            InitEquipmentSlots();

            if (_inventory == null) _inventory = gameObject.GetComponent<Inventory>();

            if (_experienceFunction == null)
            {
                _xpNeeded = 99999999;
            } else
            {
                _xpNeeded = _experienceFunction.GetXpNeeded(_level);
            }
        }

        private void InitEquipmentSlots()
        {
            int slotAmount = Enum.GetValues(typeof(EquipmentSlot)).Length;
            _equipmentSlots = new Equipment[slotAmount];
        }

        /// <summary>
        /// Recalculates all the total stat values.
        /// </summary>
        protected void UpdateStats()
        {
            _strength = _strengthMod * (_baseStrength + _strengthIncrease);
            _dexterity = _dexterityMod * (_baseDexterity + _dexterityIncrease);
            _agility = _agilityMod * (_baseAgility + _agilityIncrease);
            _intellect = _intellectMod * (_baseIntellect + _intellectIncrease);
            _spirit = _spiritMod * (_baseSpirit + _spiritIncrease);
            CharacterStatsChanged?.Invoke();
        }

        /// <summary>
        /// Equips the item to its slot and adds its stats to the player's total stat value.
        /// </summary>
        /// <param name="equipment"></param>
        public void Equip(Equipment equipment)
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
        public void Unequip(EquipmentSlot slot)
        {
            Unequip((int)slot);
        }

        /// <summary>
        /// Removes the equipment at the given slot and subtracts its stats from the character.
        /// </summary>
        /// <param name="slot"></param>
        public void Unequip(int slot)
        {
            if (_equipmentSlots == null) InitEquipmentSlots();
            Debug.Log("Unequipping slot (" + slot + ").");

            if (_equipmentSlots[slot] == null) return;
            RemoveEquipmentStats(_equipmentSlots[slot]);

            _inventory?.Add(_equipmentSlots[slot], 1);          // Place the item in the inventory if possible.
            
            _equipmentSlots[slot] = null;                       // Clear the equipment slot.
        }

        /// <summary>
        /// Removes the equipment's stats from the player's stats.
        /// </summary>
        protected void RemoveEquipmentStats(Equipment equipment)
        {
            _strengthIncrease -= equipment.Strength;
            _dexterityIncrease -= equipment.Dexterity;
            _agilityIncrease -= equipment.Agility;
            _intellectIncrease -= equipment.Intellect;
            _spiritIncrease -= equipment.Spirit;
            UpdateStats();
        }

        /// <summary>
        /// Adds the equipment's stats to the player's stats.
        /// </summary>
        /// <param name="equipment"></param>
        protected void AddEquipmentStats(Equipment equipment)
        {
            _strengthIncrease += equipment.Strength;
            _dexterityIncrease += equipment.Dexterity;
            _agilityIncrease += equipment.Agility;
            _intellectIncrease += equipment.Intellect;
            _spiritIncrease += equipment.Spirit;
            UpdateStats();
        }

        public Equipment GetEquipment(int slot)
        {
            if (slot < 0 || slot >= _equipmentSlots.Length) return null;

            return _equipmentSlots[slot];
        }

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

        public virtual void LevelUp()
        {
            _level++;

            _xp = _xp - _xpNeeded;
            _xpNeeded = _experienceFunction.GetXpNeeded(_level);

            _baseStrength += _strengthGrowth;
            _baseDexterity += _dexterityGrowth;
            _baseAgility += _agilityGrowth;
            _baseIntellect += _intellectGrowth;
            _baseSpirit += _spiritGrowth;

            if (_xp >= _xpNeeded) LevelUp();
            CharacterLeveledUp?.Invoke();

            UpdateStats();
        }
    }
}