using UnityEngine;
using TMPro;

namespace FroggyDefense.Core.Buildings.UI
{
    public class DropdownSectionUI : MonoBehaviour
    {
        [SerializeField] private Turret _turret;                    // The turret being represented.

        [SerializeField] private TextMeshProUGUI _titleText;        // The main title text.
        [SerializeField] private TextMeshProUGUI _subtitleText;     // The subtitle text.
        [SerializeField] private TextMeshProUGUI _buttonText;       // The button text.

        [SerializeField] private TMP_Dropdown _dropDown;            // The drop down menu.

        public void InitUI(Turret turret)
        {
            _turret = turret;

            UpdateUI();
        }

        public void UpdateUI()
        {

        }
    }
}
