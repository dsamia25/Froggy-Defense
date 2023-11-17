using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.UI;

namespace FroggyDefense.Core.Items.Crafting.UI
{
    public class CraftingMaterialsListUI : MonoBehaviour
    {
        private static int HOLD_AMOUNT = 4;         // How many items should be cached instead of deleted when they're not needed.

        [SerializeField] protected Transform SpawnPosition;

        [SerializeField] protected List<CraftingMaterialsListItemUI> RequiredMaterials;
        [SerializeField] protected GameObject RequiredMaterialPrefab;

        private IInventory _playerInventory => GameManager.instance.m_Player.CharacterInventory;

        private CraftingRecipe _displayedRecipe;
        public CraftingRecipe DisplayedRecipe
        {
            get => _displayedRecipe;
            set
            {
                _displayedRecipe = value;
                UpdateUI();
            }
        }

        private void Awake()
        {
            if (RequiredMaterials == null)
            {
                RequiredMaterials = new List<CraftingMaterialsListItemUI>();
            }
        }

        /// <summary>
        /// Updates the UI to have the correctly displayed values.
        /// </summary>
        public void UpdateUI()
        {
            if (_displayedRecipe == null)
            {
                return;
            }

            if (RequiredMaterials == null)
            {
                RequiredMaterials = new List<CraftingMaterialsListItemUI>();
            }

            int len = _displayedRecipe.CraftingMaterials.Length;

            ResizingList<CraftingMaterialsListItemUI>.Resize(RequiredMaterialPrefab, SpawnPosition, RequiredMaterials, len, HOLD_AMOUNT);

            for (int i = 0; i < len; i++)
            {
                var mat = _displayedRecipe.CraftingMaterials[i];
                RequiredMaterials[i].Set(mat.m_Item.Icon, _playerInventory.GetCount(mat.m_Item.Id), mat.RequiredAmount);
                RequiredMaterials[i].gameObject.SetActive(true);
            }
        }
    }
}