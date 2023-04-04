using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Shop
{
    public class ItemShopItem : ShopItem
    {
        private Item item = null;
        private int amount = 1;

        /// <summary>
        /// Adds the item to the character's inventory.
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        public override bool Buy(Character buyer)
        {
            if (buyer.CharacterInventory == null) return false;

            buyer.CharacterInventory.Add(item, amount);
            return true;
        }
    }
}