using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FroggyDefense.Core.Buildings.UI
{
    public class TurretSheetUI : MonoBehaviour
    {
        [SerializeField] private Turret _turret;
        [SerializeField] private TextMeshProUGUI _titleText;

        [SerializeField] private Transform _uiParent = null;

        [Space]
        [Header("Prefabs")]
        [Space]
        [SerializeField] private GameObject _targetOptionPrefab;
        [SerializeField] private GameObject _upgradeSectionPrefab;

        [Space]
        [Header("Upgrade Sections")]
        [Space]
        [SerializeField] private TextMeshProUGUI _upgradeDirectDamageButtonText;    // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeSplashDamageButtonText;    // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeAttackSpeedButtonText;     // The button text.
        [SerializeField] private TextMeshProUGUI _upgradeRangeButtonText;           // The button text.
        [SerializeField] private TextMeshProUGUI _directDamageText;                 // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _splashDamageText;                 // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _attackSpeedText;                  // The text displaying the turret's stats.
        [SerializeField] private TextMeshProUGUI _rangeText;                        // The text displaying the turret's stats.

        [Space]
        [Header("Sections")]
        [Space]
        [SerializeField] private DropdownSectionUI _targetOptionSection;
        [SerializeField] private List<UpgradeSectionUI> _upgradeSections;
        [SerializeField] private Dictionary<UpgradeSectionUI, TurretStat> _upgradeSectionPairings;

        public Turret m_Turret
        {
            get => _turret;
            set
            {
                if (value != _turret)
                {
                    _turret = value;
                    ClearUI();
                    InitUI();
                }
            }
        }

        private void Start()
        {
            if (_uiParent == null)
            {
                _uiParent = transform;
            }    
        }

        /// <summary>
        /// Deletes all old stuff.
        /// </summary>
        public void ClearUI()
        {
            if (_targetOptionSection != null)
            {
                Destroy(_targetOptionSection.gameObject);
            }

            if (_upgradeSections != null)
            {
                foreach (var section in _upgradeSections)
                {
                    Destroy(section.gameObject);
                }
            }
        }

        /// <summary>
        /// Initilizes new UI elements for the selected turret.
        /// </summary>
        public void InitUI()
        {
            GameObject newSection;

            // Make lists.
            _upgradeSections = new List<UpgradeSectionUI>();
            _upgradeSectionPairings = new Dictionary<UpgradeSectionUI, TurretStat>();

            // Make target options section if needed.
            if (_turret.CanSelectTargetOptions)
            {
                newSection = Instantiate(_targetOptionPrefab, _uiParent);
                _targetOptionSection = newSection.GetComponent<DropdownSectionUI>();
                // _targetOptionSection.InitUI();
            }

            // Make all needed upgrade sections.
            for (int i = 0; i < _turret.UpgradeOptions.Length; i++)
            {
                newSection = Instantiate(_upgradeSectionPrefab, _uiParent);
                var upgradeSection = newSection.GetComponent<UpgradeSectionUI>();
                upgradeSection.InitUI(_turret, _turret.UpgradeOptions[i]);
                _upgradeSections.Add(upgradeSection);
                _upgradeSectionPairings.Add(upgradeSection, _turret.UpgradeOptions[i]);
            }
        }

        /// <summary>
        /// Updates the TurretSheetUI.
        /// </summary>
        public void UpdateUI()
        {
            _titleText.text = _turret.TurretUpgradeSheet.Name;

            if (_targetOptionSection != null)
            {
                _targetOptionSection.UpdateUI();
            }

            foreach (var section in _upgradeSections)
            {
                section.UpdateUI();
            }
            // Stat Text
            //_directDamageText.text = StatStringFormat(_turret.m_DirectDamage,
            //    (_turret.DirectDamageLevel < _turret.MaxDirectDamageLevel ? _turret.TurretUpgradeSheet.DirectDamageUpgradeValues[_turret.DirectDamageLevel] : -1));
            //_splashDamageText.text = StatStringFormat(_turret.m_SplashDamage,
            //    (_turret.SplashDamageLevel < _turret.MaxSplashDamageLevel ? _turret.TurretUpgradeSheet.SplashDamageUpgradeValues[_turret.SplashDamageLevel] : -1));
            //_rangeText.text = StatStringFormat(_turret.m_TargetRadius,
            //    (_turret.RangeLevel < _turret.MaxRangeLevel ? _turret.TurretUpgradeSheet.RangeUpgradeValues[_turret.RangeLevel] : -1));

            //// Upgrade Button Text
            //_upgradeDirectDamageButtonText.text = "Upgrade Direct Damage" + (_turret.DirectDamageLevel < _turret.MaxDirectDamageLevel ? "(" + _turret.TurretUpgradeSheet.DirectDamageUpgradeCosts[_turret.DirectDamageLevel].ToString() + " Gems)" : "");
            //_upgradeSplashDamageButtonText.text = "Upgrade Splash Damage" + (_turret.SplashDamageLevel < _turret.MaxSplashDamageLevel ? "(" + _turret.TurretUpgradeSheet.SplashDamageUpgradeCosts[_turret.SplashDamageLevel].ToString() + " Gems)" : "");
            //_upgradeRangeButtonText.text = "Upgrade Range" + (_turret.RangeLevel < _turret.MaxRangeLevel ? "(" + _turret.TurretUpgradeSheet.RangeUpgradeCosts[_turret.RangeLevel].ToString() + " Gems)": "");
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
        /// Upgrades the turret then updates the UI.
        /// </summary>
        public void UpgradeButton(TurretStat upgrade)
        {
            switch (upgrade)
            {
                case TurretStat.DirectDamage:
                    _turret.UpgradeDirectDamage();
                    break;
                case TurretStat.SplashDamage:
                    _turret.UpgradeSplashDamage();
                    break;
                case TurretStat.AttackSpeed:
                    _turret.UpgradeAttackSpeed();
                    break;
                case TurretStat.Range:
                    _turret.UpgradeRange();
                    break;
                default:
                    Debug.LogWarning("Unknown TurretUpgradeOption (" + upgrade.ToString() + ").");
                    break;
            }
            UpdateUI();
        }
    }
}