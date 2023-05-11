using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    // TODO: Finish transitoning stats from Character to StatSheet.
    public class StatSheet
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
        [Header("Base Stat Growth")]
        [Space]
        [SerializeField] private float _strengthGrowth = 1f;       // How much of this stat is added on leveling up.
        [SerializeField] private float _dexterityGrowth = 1f;      // How much of this stat is added on leveling up.
        [SerializeField] private float _agilityGrowth = 1f;        // How much of this stat is added on leveling up.
        [SerializeField] private float _intellectGrowth = 1f;      // How much of this stat is added on leveling up.
        [SerializeField] private float _spiritGrowth = 1f;         // How much of this stat is added on leveling up.
    }
}