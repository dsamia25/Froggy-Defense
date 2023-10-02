using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Items.Crafting.UI
{
    public class CraftingMaterialsListItemUI : MonoBehaviour
    {
        [SerializeField] private Color AvailableColor;
        [SerializeField] private Color UnavailableColor;
        [SerializeField] private Image ColorBackGround;
        [SerializeField] private TextMeshProUGUI NumeratorText;
        [SerializeField] private TextMeshProUGUI DenominatorText;

        /// <summary>
        /// Sets the text and sets color to be gray if available and red if not.
        /// </summary>
        public void Set(int numerator, int denominator)
        {
            NumeratorText.text = numerator.ToString();
            DenominatorText.text = denominator.ToString();

            ColorBackGround.color = (numerator >= denominator ? AvailableColor : UnavailableColor);
        }
    }
}