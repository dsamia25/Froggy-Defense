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
        Spirit
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
        [Space]
        [Header("Level")]
        [Space]
        [SerializeField] private int _level = 1;
        public int Level { get => _level; set => _level = value; }

        [SerializeField] private float _moveSpeed;
        public float MoveSpeed { get => _moveSpeed; }

        [Space]
        [Header("Base Stats Per Level")]
        [Space]
        [SerializeField] private float _strengthPerLevel = 1f;        // How much of this stat is added on leveling up.
        [SerializeField] private float _dexterityPerLevel = 1f;       // How much of this stat is added on leveling up.
        [SerializeField] private float _agilityPerLevel = 1f;         // How much of this stat is added on leveling up.
        [SerializeField] private float _intellectPerLevel = 1f;       // How much of this stat is added on leveling up.
        [SerializeField] private float _spiritPerLevel = 1f;          // How much of this stat is added on leveling up.
        public float BaseStrengthPerLevel => _strengthPerLevel;
        public float BaseDexterityPerLevel => _dexterityPerLevel;
        public float BaseAgilityPerLevel => _agilityPerLevel;
        public float BaseIntellectPerLevel => _intellectPerLevel;
        public float BaseSpiritPerLevel => _spiritPerLevel;

        private Dictionary<StatType, float> _baseStats = new Dictionary<StatType, float>();     // Dictionary of each base stat and their value.
        private Dictionary<StatType, float> _bonusStats = new Dictionary<StatType, float>();    // Dictionary of each increased stat and their values.
        private Dictionary<StatType, float> _statModifiers = new Dictionary<StatType, float>(); // Dictionary of each increased stat and their values.
        private Dictionary<StatType, float> _totalStats = new Dictionary<StatType, float>();    // Dictionary of each increased stat and their values.

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
            SetBaseStat(StatType.Strength, _level * _strengthPerLevel);
            SetBaseStat(StatType.Dexterity, _level * _dexterityPerLevel);
            SetBaseStat(StatType.Agility, _level * _agilityPerLevel);
            SetBaseStat(StatType.Intellect, _level * _intellectPerLevel);
            SetBaseStat(StatType.Spirit, _level * _spiritPerLevel);
            UpdateStats();
        }

        /// <summary>
        /// Increases the character's level and increases its base stats.
        /// </summary>
        public void LevelUp()
        {
            _level++;
            SetBaseStat(StatType.Strength, _level * _strengthPerLevel);
            SetBaseStat(StatType.Dexterity, _level * _dexterityPerLevel);
            SetBaseStat(StatType.Agility, _level * _agilityPerLevel);
            SetBaseStat(StatType.Intellect, _level * _intellectPerLevel);
            SetBaseStat(StatType.Spirit, _level * _spiritPerLevel);
            UpdateStats();
        }
    }
}