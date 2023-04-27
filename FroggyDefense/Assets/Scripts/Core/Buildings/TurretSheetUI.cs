using UnityEngine;
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
        [SerializeField] private TextMeshProUGUI _upgradeDirectDamageButton;
        [SerializeField] private TextMeshProUGUI _upgradeSplashDamageButton;
        [SerializeField] private TextMeshProUGUI _upgradeRangeButton;


        public Turret m_Turret
        {
            get => _turret;
            set
            {
                _turret = value;
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            _titleText.text = _turret.TurretUpgradeSheet.Name;
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeDirectDamageButton()
        {
            _turret.UpgradeDirectDamage();
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeSplashDamageButton()
        {
            _turret.UpgradeSplashDamage();
        }

        /// <summary>
        /// Tries to upgrade the turret.
        /// </summary>
        public void UpgradeRangeButton()
        {
            _turret.UpgradeRange();
        }
    }
}