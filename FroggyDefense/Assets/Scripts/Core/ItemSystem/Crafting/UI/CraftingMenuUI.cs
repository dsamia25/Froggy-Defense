using UnityEngine;
using FroggyDefense.Core.Items.UI;

namespace FroggyDefense.Core.Items.Crafting.UI {
    public class CraftingMenuUI : MonoBehaviour
    {
        // Prefabs
        [SerializeField] private GameObject CraftingRecipeIconPrefab;           // The prefab for making a crafting recipe.
        [SerializeField] private GameObject RequiredMaterialPrefab;             // The prefab for required material components.

        // Main Components
        [SerializeField] private GameObject CraftingRecipeListSection;                      // The section for making the list of crafting recipes.
        [SerializeField] private CraftingMaterialsListUI RequiredMaterialsListSection;      // The section for making the list of required materials.
        [SerializeField] private ItemViewUI ItemViewSection;                                // The section for displaying the selected item.

        // State Variables
        private IInventory _playerInventory;
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
                    RequiredMaterialsListSection.DisplayedRecipe = _selectedRecipe;
                    UpdateUI();
                }
            }
        }

        private void Start()
        {
            _playerInventory = GameManager.instance.m_Player.GetComponent<IInventory>();
            CraftingSelectionBarItemUI.OnSelectedEvent += OnRecipeSelected;    
        }

        /// <summary>
        /// Updates all parts of the menu's UI.
        /// </summary>
        private void UpdateUI()
        {
            if (_selectedRecipe == null)
            {
                ItemViewSection.gameObject.SetActive(false);
                return;
            }

            ItemViewSection.UpdateUI();
            RequiredMaterialsListSection.UpdateUI();
            ItemViewSection.gameObject.SetActive(true);
        }

        /// <summary>
        /// When the user clicks on a new recipe item.
        /// </summary>
        private void OnRecipeSelected(CraftingRecipe recipe)
        {
            SelectedRecipe = recipe;
        }

        /// <summary>
        /// Tries to craft the item whe nthe button is clicked.
        /// </summary>
        public void OnCraftButtonClicked()
        {
            if (_selectedRecipe == null) return;
            if (_playerInventory == null) return;

            CraftingUtil.Craft(_selectedRecipe, _playerInventory);
            UpdateUI();
        }
    }
}