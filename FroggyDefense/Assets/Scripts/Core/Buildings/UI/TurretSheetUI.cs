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
        [Header("Sections")]
        [Space]
        [SerializeField] private DropdownSectionUI _targetOptionSection;
        [SerializeField] private List<UpgradeSectionUI> _upgradeSections;

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

            _titleText.text = _turret.TurretUpgradeSheet.Name;

            // Make lists.
            _upgradeSections = new List<UpgradeSectionUI>();

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
        }
    }
}