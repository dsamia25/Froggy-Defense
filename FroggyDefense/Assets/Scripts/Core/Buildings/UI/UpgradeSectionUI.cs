using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Buildings.UI
{
    public class UpgradeSectionUI : MonoBehaviour
    {   
        [SerializeField] private Turret _turret;                    // The turret being represented.
        [SerializeField] private TurretStat _statType;              // The stat type being represented.

        [SerializeField] private TextMeshProUGUI _titleText;        // The main title text.
        [SerializeField] private TextMeshProUGUI _subtitleText;     // The subtitle text.
        [SerializeField] private Button _button;                    // The upgrade button.
        [SerializeField] private TextMeshProUGUI _buttonText;       // The button text.

        public void InitUI(Turret turret, TurretStat statType)
        {
            _turret = turret;
            _statType = statType;

            _button.onClick.AddListener(
                () => {
                    _turret.UpgradeTurret(statType);
                    UpdateUI();
                }
            );

            UpdateUI();
        }

        public void UpdateUI()
        {
            // Update the title and level value.
            _titleText.text = _statType.ToString() + " (" + _turret.GetStatLevel(_statType) + "/" + _turret.GetStatMaxLevel(_statType) + ")";

            // Update the stat value text.
            _subtitleText.text = StatStringFormat(_turret.GetStat(_statType),
                (IsUnderMaxLevel() ? _turret.GetUpgradeValue(_statType) : -1));

            // Update upgrade button. If at max level, disable it.
            if (IsUnderMaxLevel())
            {
                _button.gameObject.SetActive(true);
                _buttonText.text = "Upgrade" + (_turret.GetStatLevel(_statType) < _turret.GetStatMaxLevel(_statType) ? " (" + _turret.GetUpgradeCost(_statType) + " Gems)" : "");
            } else
            {
                _button.gameObject.SetActive(false);
            }
        }

        private bool IsUnderMaxLevel()
        {
            return _turret.GetStatLevel(_statType) < _turret.GetStatMaxLevel(_statType);
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
            return firstNum.ToString() + (secondNum > 0 ? " (+" + secondNum.ToString() + ")" : "");
        }
    }
}