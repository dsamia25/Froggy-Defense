using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items.Crafting
{
    public enum CraftingStationType
    {
        NULL,
        CraftingTable,
        CookingFire,
        Forge
    }

    public class CraftingStation : MonoBehaviour
    {
        [SerializeField] private CraftingStationType StationType;

        /// <summary>
        /// Attempts to craft the item using the reagents from the given inventory.
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool Craft(CraftingRecipe recipe, IInventory inventory)
        {
            if (!recipe.CanCraft(inventory))
            {
                Debug.Log($"Cannot craft {recipe.Name}.");
                return false;
            }

            try
            {
                Item craftedItem = Item.CreateItem(recipe.Created);
                foreach (var reagent in recipe.CraftingMaterials)
                {
                    //inventory.Subtract(reagent.m_Item, reagent.RequiredAmount);
                }
                inventory.Add(craftedItem, 1);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error crafting recipe: {e}");
            }
            return true;
        }
    }
}