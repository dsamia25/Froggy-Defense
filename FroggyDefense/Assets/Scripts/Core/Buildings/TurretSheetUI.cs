using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Buildings.UI
{
    public class TurretSheetUI : MonoBehaviour
    {
        [SerializeField] private Turret _turret;
        [SerializeField] private TextMeshProUGUI _titleText;

        [Space]
        [Header("Target Options Section")]
        [Space]
        [SerializeField] private TextMeshProUGUI _targetOptionDropdown;

        [Space]
        [Header("Upgrade Sections")]
        [Space]
        [SerializeField] private Button _upgradeDirectDamageButton;                 // The button to upgrade the turret.
        [SerializeField] private Button _upgradeSplashDamageButton;                 // The button to upgrade the turret.
        [SerializeField] private Button _upgradeAttackSpeedButton;                  // The button to upgrade the turret.
        [SerializeField] private Button _upgradeRangeButton;                        // The button to upgrade the turret.
        [SerializeField] private TextMeshProUGUI _upgradeDirectDamageButtonText;    // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeSplashDamageButtonText;    // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeAttackSpeedButtonText;     // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeRangeButtonText;           // The button text.
        [SerializeField] private TextMeshProUGUI _directDamageText;                 // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _splashDamageText;                 // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _attackSpeedText;                  // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _rangeText;                        // The text displaying the turret's stats.

        public Turret m_Turret
        {
            get => _turret;
            set
            {
                _turret = value;
                UpdateUI();
            }
        }

        // TODO: Add attack speed text updates.
        // TODO: Change the button colors based on if they can be bought.
        /// <summary>
        /// Updates the TurretSheetUI.
        /// </summary>
        public void UpdateUI()
        {
            _titleText.text = _turret.TurretUpgradeSheet.Name;

            // Stat Text
            _directDamageText.text = StatStringFormat(_turret.m_DirectDamage,
                (_turret.DirectDamageLevel < _turret.MaxDirectDamageLevel ? _turret.TurretUpgradeSheet.DirectDamageUpgradeValues[_turret.DirectDamageLevel] : -1));
            _splashDamageText.text = StatStringFormat(_turret.m_SplashDamage,
                (_turret.SplashDamageLevel < _turret.MaxSplashDamageLevel ? _turret.TurretUpgradeSheet.SplashDamageUpgradeValues[_turret.SplashDamageLevel] : -1));
            _rangeText.text = StatStringFormat(_turret.m_TargetRadius,
                (_turret.RangeLevel < _turret.MaxRangeLevel ? _turret.TurretUpgradeSheet.RangeUpgradeValues[_turret.RangeLevel] : -1));

            // Upgrade Button Text
            _upgradeDirectDamageButtonText.text = "Upgrade Direct Damage" + (_turret.DirectDamageLevel < _turret.MaxDirectDamageLevel ? "(" + _turret.TurretUpgradeSheet.DirectDamageUpgradeCosts[_turret.DirectDamageLevel].ToString() + " Gems)" : "");
            _upgradeSplashDamageButtonText.text = "Upgrade Splash Damage" + (_turret.SplashDamageLevel < _turret.MaxSplashDamageLevel ? "(" + _turret.TurretUpgradeSheet.SplashDamageUpgradeCosts[_turret.SplashDamageLevel].ToString() + " Gems)" : "");
            _upgradeRangeButtonText.text = "Upgrade Range" + (_turret.RangeLevel < _turret.MaxRangeLevel ? "(" + _turret.TurretUpgradeSheet.RangeUpgradeCosts[_turret.RangeLevel].ToString() + " Gems)": "");
        }

        /// <summary>
        /// Puts the turret stats in the correct format with the current value and
        /// how much the upgrade will add on.
        /// </summary>
        /// <param name="firstNum"></param>
        /// <param name="secondNum"></param>
        /// <returns></returns>
        private string StatStringFormat(float firstNum, float secondNum)
        {
            return firstNum.ToString() + (secondNum > 0 ? " (+" + secondNum.ToString() + ")": "");
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeDirectDamageButton()
        {
            _turret.UpgradeDirectDamage();
            UpdateUI();
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeSplashDamageButton()
        {
            _turret.UpgradeSplashDamage();
            UpdateUI();
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeAttackSpeedButton()
        {
            _turret.UpgradeAttackSpeed();
            UpdateUI();
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeRangeButton()
        {
            _turret.UpgradeRange();
            UpdateUI();
        }
    }
}