using UnityEngine;
using FroggyDefense.Core.Items.UI;

namespace FroggyDefense.Core.Items.Crafting.UI {
    public class CraftingMenuUI : MonoBehaviour
    {
        // Prefabs
        [SerializeField] private GameObject CraftingRecipeIconPrefab;           // The prefab for making a crafting recipe.
        [SerializeField] private GameObject RequiredMaterialPrefab;             // The prefab for required material components.

        // Main Components
        [SerializeField] private GameObject CraftingRecipeListSection;          // The section for making the list of crafting recipes.
        [SerializeField] private GameObject RequiredMaterialsListSection;       // The section for making the list of required materials.
        [SerializeField] private ItemViewUI ItemViewSection;                    // The section for displaying the selected item.

        // State Variables
        [SerializeField] private CraftingRecipe _selectedRecipe;                // Which recipe to display.
        public CraftingRecipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (value != null)
                {
                    _selectedRecipe = value;
                    ItemViewSection.DisplayedItem = Item.CreateItem(_selectedRecipe.Created);   // TODO: This is a temp fix. Should make ItemViewUI able to display ItemObjects too.
                }
            }
        }

        /// <summary>
        /// Updates all parts of the menu's UI.
        /// </summary>
        private void UpdateUI()
        {

        }
    }
}