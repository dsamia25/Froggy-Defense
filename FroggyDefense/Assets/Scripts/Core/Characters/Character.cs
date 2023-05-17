using System;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core.Items;
using FroggyDefense.Economy;

namespace FroggyDefense.Core
{
    public class Character : MonoBehaviour
    {
        [Space]
        [Header("Character Info")]
        [Space]
        [SerializeField] private string _name = "NAME HERE";    
        public string Name { get => _name; }                    // The character's name.

        [Space]
        [Header("Experience")]
        [Space]
        [SerializeField] private float _xp = 0f;
        [SerializeField] private float _xpNeeded = 0f;
        [SerializeField] private CharacterLevelExperienceFunction _experienceFunction = null;
        public float Xp { get => _xp; }                         // The character's experience points.
        public float XpNeeded { get => _xpNeeded; }             // The amount of xp required to level up.
        public CharacterLevelExperienceFunction ExperienceFunction { get => _experienceFunction; }  // Function used to calculate experience needed to level up.

        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected StatSheet _stats;              // List of base stats and their growth per level.
        public StatSheet Stats => _stats;

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
                _xpNeeded = _experienceFunction.GetXpNeeded(_stats.Level);
            }
            _stats.SetLevel(1);
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
            _stats.UpdateStats();
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

        // TODO: Make the stat increases in equipment a list that can be dynamically sorted through instead of hard-coding it like this.
        /// <summary>
        /// Removes the equipment's stats from the player's stats.
        /// </summary>
        protected void RemoveEquipmentStats(Equipment equipment)
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
    }
}