using UnityEngine;

namespace FroggyDefense.Core.Buildings
{
    [CreateAssetMenu(fileName = "New Turret Upgrade Sheet", menuName = "ScriptableObjects/Buildings/Turrets/Turret Upgrade Sheet")]
    public class TurretUpgradeSheetObject : ScriptableObject
    {
        [Space]
        [Header("Properties")]
        [Space]
        public string Name = "TURRET";                  // The turret's name.
        public string Description = "A NEW TURRET";     // The turret's description.
        public int BuildPrice = 3;                      // How much it costs to build this turret.

        [Space]
        [Header("Upgrades")]
        [Space]
        [SerializeField] private int[] _directDamageUpgradeCosts = { 15, 20, 30, 50, 90 };
        [SerializeField] private int[] _splashDamageUpgradeCosts = { 15, 20, 30, 50, 90 };
        [SerializeField] private int[] _rangeUpgradeCosts = { 15, 20, 30, 50, 90 };
        public int[] DirectDamageUpgradeCosts { get => _directDamageUpgradeCosts; }
        public int[] SplashDamageUpgradeCosts { get => _splashDamageUpgradeCosts; }
        public int[] RangeUpgradeCosts { get => _rangeUpgradeCosts; }

        [SerializeField] private int[] _directDamageUpgradeValues = { 1, 1, 2, 3, 5 };
        [SerializeField] private int[] _splashDamageUpgradeValues = { 1, 1, 2, 3, 5 };
        [SerializeField] private int[] _rangeUpgradeValues = { 1, 1, 1, 1, 1 };
        public int[] DirectDamageUpgradeValues { get => _directDamageUpgradeValues; }
        public int[] SplashDamageUpgradeValues { get => _splashDamageUpgradeValues; }
        public int[] RangeUpgradeValues { get => _rangeUpgradeValues; }
    }
}