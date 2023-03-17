using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Items;

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
        [Header("Base Stats")]
        [Space]
        [SerializeField] private float _baseStrength = 1f;      // Base stats.
        [SerializeField] private float _baseDexterity = 1f;     // Base stats.
        [SerializeField] private float _baseAgility = 1f;       // Base stats.
        [SerializeField] private float _baseIntellect = 1f;     // Base stats.
        [SerializeField] private float _baseSpirit = 1f;        // Base stats.

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

        // TODO: Maybe change to an array? Would use the index to equip items.
        [Space]
        [Header("Equipment")]
        [Space]
        public Equipment HatSlot = null;
        public Equipment ClothesSlot = null;
        public Equipment BootsSlot = null;
        public Equipment RingSlot = null;
        public Equipment TrinketSlot = null;

        // TODO: Check if need these.
        // TODO: Probably remove these and the classes entirely.
        [Space]
        [Header("Buffs")]
        [Space]
        public List<Buff> Buffs = new List<Buff>();                         // List of all buffs with references to their effects.
        public List<ValueBuff> ValueBuffs = new List<ValueBuff>();          // List of all value buffs.
        public List<PercentBuff> PercentBuffs = new List<PercentBuff>();    // List of all percent buffs.

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
        }

        /// <summary>
        /// Equips the item to its slot and adds its stats to the player's total stat value.
        /// </summary>
        /// <param name="equipment"></param>
        public void Equip(Equipment equipment)
        {
            switch (equipment.Slot)
            {
                case EquipmentSlot.Hat:
                    if (HatSlot != null) Unequip(EquipmentSlot.Hat);
                    HatSlot = equipment;
                    AddEquipmentStats(HatSlot);
                    return;
                case EquipmentSlot.Clothes:
                    if (ClothesSlot != null) Unequip(EquipmentSlot.Clothes);
                    ClothesSlot = equipment;
                    AddEquipmentStats(ClothesSlot);
                    return;
                case EquipmentSlot.Boots:
                    if (BootsSlot != null) Unequip(EquipmentSlot.Boots);
                    BootsSlot = equipment;
                    AddEquipmentStats(BootsSlot);
                    return;
                case EquipmentSlot.Ring:
                    if (RingSlot != null) Unequip(EquipmentSlot.Ring);
                    RingSlot = equipment;
                    AddEquipmentStats(RingSlot);
                    return;
                case EquipmentSlot.Trinket:
                    if (TrinketSlot != null) Unequip(EquipmentSlot.Trinket);
                    TrinketSlot = equipment;
                    AddEquipmentStats(TrinketSlot);
                    return;
                default:
                    Debug.LogWarning("Attempting to equip unknown item.");
                    return;
            }
        }

        /// <summary>
        /// Removes the equipment at the given slot and subtracts its stats from the character.
        /// </summary>
        /// <param name="slot"></param>
        public void Unequip(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Hat:
                    if (HatSlot == null) return;
                    RemoveEquipmentStats(HatSlot);
                    HatSlot = null;
                    return;
                case EquipmentSlot.Clothes:
                    if (ClothesSlot == null) return;
                    RemoveEquipmentStats(ClothesSlot);
                    ClothesSlot = null;
                    return;
                case EquipmentSlot.Boots:
                    if (BootsSlot == null) return;
                    RemoveEquipmentStats(BootsSlot);
                    BootsSlot = null;
                    return;
                case EquipmentSlot.Ring:
                    if (RingSlot == null) return;
                    RemoveEquipmentStats(RingSlot);
                    RingSlot = null;
                    return;
                case EquipmentSlot.Trinket:
                    if (TrinketSlot == null) return;
                    RemoveEquipmentStats(TrinketSlot);
                    TrinketSlot = null;
                    return;
                default:
                    Debug.LogWarning("Attempting to unequip unknown item.");
                    return;
            }
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
    }
}