using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Items.Crafting.UI
{
    public class CraftingMaterialsListItemUI : MonoBehaviour
    {
        [SerializeField] private Color AvailableColor;
        [SerializeField] private Color UnavailableColor;
        [SerializeField] private Image Icon;
        [SerializeField] private Image ColorBackground;
        [SerializeField] private Image NumberBackground;
        [SerializeField] private TextMeshProUGUI NumeratorText;
        [SerializeField] private TextMeshProUGUI DenominatorText;

        /// <summary>
        /// Sets the text and sets color to be gray if available and red if not.
        /// </summary>
        public void Set(Sprite icon, int numerator, int denominator)
        {
            Icon.sprite = icon;
            NumeratorText.text = numerator.ToString();
            DenominatorText.text = denominator.ToString();

            Color color = (numerator >= denominator ? AvailableColor : UnavailableColor);
            ColorBackground.color = color;
            NumberBackground.color = color;
        }
    }
}