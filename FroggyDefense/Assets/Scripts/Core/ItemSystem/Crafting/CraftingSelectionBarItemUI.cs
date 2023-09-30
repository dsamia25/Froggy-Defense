using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Items.Crafting.UI {
    public class CraftingSelectionBarItemUI : MonoBehaviour
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI NameText;

        [SerializeField] private CraftingRecipe _recipe;
        public CraftingRecipe Recipe
        {
            get => _recipe;
            set
            {
                if (value != null) {
                    _recipe = value;
                    UpdateUI();
                }
            }
        }

        public delegate void CraftingSelectionBarItemDelegate(CraftingRecipe recipe);
        public static event CraftingSelectionBarItemDelegate OnSelectedEvent;

        private void Start()
        {
            UpdateUI();
        }

        public void OnSelected()
        {
            OnSelectedEvent?.Invoke(Recipe);
        }

        private void UpdateUI()
        {
            Icon.sprite = _recipe.Created.Icon;
            NameText.text = _recipe.Created.Name;
        }
    }
}