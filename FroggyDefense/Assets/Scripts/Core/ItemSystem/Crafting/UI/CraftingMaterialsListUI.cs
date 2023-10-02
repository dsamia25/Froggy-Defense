using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items.Crafting.UI
{
    public class CraftingMaterialsListUI : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> RequiredMaterials;
        [SerializeField] protected GameObject RequiredMaterialPrefab;

        private Item _displayedItem;
        public Item DisplayedItem
        {
            get => _displayedItem;
            set
            {
                _displayedItem = value;
                UpdateUI();
            }
        }

        /// <summary>
        /// Updates the UI to have the correctly displayed values.
        /// </summary>
        public void UpdateUI()
        {

        }
    }
}