using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    public enum StatType
    {
        Strength,
        Dexterity,
        Agility,
        Intellect,
        Spirit,
        Physical_Spell_Damage,
        Poison_Spell_Damage,
        Bleed_Spell_Damage,
        Fire_Spell_Damage,
        Frost_Spell_Damage,
        Spirit_Spell_Damage
    }

    [System.Serializable]
    public struct StatValuePair
    {
        public StatType Stat;
        public float Value;

        public StatValuePair(StatType stat, float value)
        {
            Stat = stat;
            Value = value;
        }

        public override string ToString()
        {
            return Stat.ToString() + ": " + Value;
        }
    }

    [System.Serializable]
    public struct StatValueRange
    {
        public StatType Stat;
        public Vector2Int ValueRange;
        public int Value { get => UnityEngine.Random.Range(ValueRange.x, ValueRange.y); }

        public StatValueRange(StatType stat, Vector2Int valueRange)
        {
            Stat = stat;
            ValueRange = valueRange;
        }
    }

    [System.Serializable]
    public class StatSheet
    {
        [SerializeField] private int _level = 1;
        public int Level { get => _level; set => _level = value; }

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _healthRegen;
        [SerializeField] private float _combatHealthRegenModifier = .5f;
        [SerializeField] private float _maxMana;
        [SerializeField] private float _manaRegen;
        [SerializeField] private float _combatManaRegenModifier = .5f;
        [SerializeField] private float _attackPower;
        [SerializeField] private float _spellPower;
        [SerializeField] private float _critChance;
        [SerializeField] private float _critBonus;
        [SerializeField] private float _damagedInvincibilityTime;

        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float HealthRegen => _healthRegen;
        public float CombatHealthRegenModifier => _combatHealthRegenModifier;
        public float MaxMana => _maxMana;
        public float ManaRegen => _manaRegen;
        public float CombatManaRegenModifier => _combatManaRegenModifier;
        public float AttackPower => _attackPower;
        public float SpellPower => _spellPower;
        public float CritChance => _critChance;
        public float CritBonus => _critBonus;
        public float DamagedInvincibilityTime => _damagedInvincibilityTime;

        public Dictionary<DamageType, float> SpellPowerIndex = new Dictionary<DamageType, float>(); // Index holding additional spell power bonuses for each type of damage. Defaults to 0.

        // Stamina, Fortitude, Resilience, Endurance => Tankiness
        // Strength, Courage, Might => Power
        // Agility, Dexterity, Finesse, Artistry => Combos
        // Intellect => Magic / Utility
        // Spirit, Essence, Vitality, Will => Regeneration

        [SerializeField] private float _stamina = 1f;
        [SerializeField] private float _strength = 1f;
        [SerializeField] private float _agility = 1f;
        [SerializeField] private float _intellect = 1f;
        [SerializeField] private float _spirit = 1f;
        public float Stamina => _stamina;
        public float Strength => _strength;
        public float Agility => _agility;
        public float Intellect => _intellect;
        public float Spirit => _spirit;

        private Dictionary<StatType, float> _baseStats = new Dictionary<StatType, float>();     // Dictionary of each base stat and their value.
        private Dictionary<StatType, float> _bonusStats = new Dictionary<StatType, float>();    // Dictionary of each increased stat and their values.
        private Dictionary<StatType, float> _statModifiers = new Dictionary<StatType, float>(); // Dictionary of each increased stat and their values.
        private Dictionary<StatType, float> _totalStats = new Dictionary<StatType, float>();    // Dictionary of each increased stat and their values.

        /// <summary>
        /// Gets the total spell power for the type of spell school.
        /// </summary>
        /// <returns></returns>
        public float GetSpellPower(DamageType type)
        {
            if (!SpellPowerIndex.ContainsKey(type))
            {
                SpellPowerIndex.Add(type, 0);
            }
            return SpellPower + SpellPowerIndex[type];
        }

        /// <summary>
        /// Recalculates all the total stat values.
        /// </summary>
        public void UpdateStats()
        {
            Dictionary<StatType, float> newTotal = new Dictionary<StatType, float>();
            foreach (StatType stat in _baseStats.Keys)
            {
                float total = _baseStats[stat];
                if (_bonusStats.ContainsKey(stat))
                {
                    total += _bonusStats[stat];
                }
                if (_statModifiers.ContainsKey(stat))
                {
                    total *= _statModifiers[stat];
                }

                newTotal.TryAdd(stat, total);
            }
            _totalStats = newTotal;
        }

        /// <summary>
        /// Sets the base stat value to the set mount.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        private void SetBaseStat(StatType stat, float amount)
        {
            if (_baseStats.ContainsKey(stat))
            {
                _baseStats[stat] = amount;
            } else
            {
                _baseStats.TryAdd(stat, amount);
            }
        }

        /// <summary>
        /// Increases the stat by the amount.
        /// Set percentIncrease to true to make the value a percent modifier.
        /// For a normal stat increase set percentIncrease to false.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void AddBonusStat(StatType stat, float amount, bool percentIncrease)
        {
            if (percentIncrease)
            {
                if (_statModifiers.ContainsKey(stat))
                {
                    _statModifiers[stat] += amount;
                }
                else
                {
                    // Default is 1.0f for 100%.
                    _statModifiers.TryAdd(stat, 1.0f + amount);
                }
            } else
            {
                if (_bonusStats.ContainsKey(stat))
                {
                    _bonusStats[stat] += amount;
                }
                else
                {
                    _bonusStats.TryAdd(stat, amount);
                }
            }
            UpdateStats();
        }

        /// <summary>
        /// Decreases the stat by the amount.
        /// Set percentIncrease to true to make the value a percent modifier.
        /// For a normal stat decrease set percentIncrease to false.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void RemoveBonusStat(StatType stat, float amount, bool percentDecrease)
        {
            if (percentDecrease)
            {
                if (_statModifiers.ContainsKey(stat))
                {
                    // Make sure stat modifer doesn't go below 0.
                    if (_statModifiers[stat] - amount < 0)
                    {
                        _statModifiers[stat] = 0;
                    }
                    else
                    {
                        _statModifiers[stat] -= amount;
                    }
                }
            } else
            {
                if (_bonusStats.ContainsKey(stat))
                {
                    // Make sure increased stats doesn't go below 0.
                    if (_bonusStats[stat] - amount < 0)
                    {
                        _bonusStats[stat] = 0;
                    }
                    else
                    {
                        _bonusStats[stat] -= amount;
                    }
                }
            }
            UpdateStats();
        }

        /// <summary>
        /// Returns the base stat value.
        /// Returns 0 as default value.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetBaseStat(StatType stat)
        {
            if (_baseStats.ContainsKey(stat))
            {
                return _baseStats[stat];
            }
            return 0;
        }

        /// <summary>
        /// Returns the increased stat value.
        /// Returns 0 as default value.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetBonusStat(StatType stat)
        {
            if (_bonusStats.ContainsKey(stat))
            {
                return _bonusStats[stat];
            }
            return 0;
        }

        /// <summary>
        /// Returns the stat percent modifer.
        /// Returns 1 as default value.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetStatModifer(StatType stat)
        {
            if (_statModifiers.ContainsKey(stat))
            {
                return _statModifiers[stat];
            }
            return 1;
        }

        /// <summary>
        /// Returns the total stat value including percent modifiers.
        /// Returns 0 as default value.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetTotalStat(StatType stat)
        {
            if (_totalStats.ContainsKey(stat))
            {
                return _totalStats[stat];
            }
            return 0;
        }

        /// <summary>
        /// Sets the stats to be at the given level.
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(int level)
        {
            _level = Level;
            SetBaseStat(StatType.Dexterity, _level);
            SetBaseStat(StatType.Strength, _level);
            SetBaseStat(StatType.Agility, _level);
            SetBaseStat(StatType.Intellect, _level);
            SetBaseStat(StatType.Spirit, _level);
            UpdateStats();
        }

        /// <summary>
        /// Increases the character's level and increases its base stats.
        /// </summary>
        public void LevelUp()
        {
            _level++;
            SetBaseStat(StatType.Dexterity, _level);
            SetBaseStat(StatType.Strength, _level);
            SetBaseStat(StatType.Agility, _level);
            SetBaseStat(StatType.Intellect, _level);
            SetBaseStat(StatType.Spirit, _level);
            UpdateStats();
        }
    }
}