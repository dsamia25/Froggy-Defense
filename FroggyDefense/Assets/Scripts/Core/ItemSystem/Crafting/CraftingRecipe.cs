using UnityEngine;

namespace FroggyDefense.Core.Items.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "ScriptableObjects/ItemSystem/Crafting/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public ItemRequirement[] CraftingMaterials;         // The materials needed to make the item.
        public ItemObject Created;                          // The item created for this recipe.

        public string Name => Created.Name;

        public CraftingStationType NeededCraftingStation = CraftingStationType.NULL;

        /// <summary>
        /// Checks if the recipe can be created with the current 
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool CanCraft(IInventory inventory)
        {
            foreach (ItemRequirement mat in CraftingMaterials)
            {
                //if (inventory.GetCount(mat.m_Item) <= mat.RequiredAmount)
                //{
                //    return false;
                //}
            }
            return true;
        }

        [System.Serializable]
        public class ItemRequirement
        {
            [SerializeField] private ItemObject _item;
            [SerializeField] private int _amount;

            public ItemObject m_Item { get => _item; }
            public int RequiredAmount { get => _amount; }

            public ItemRequirement(ItemObject item, int amount)
            {
                _item = item;
                _amount = amount;
            }
        }
    }
}