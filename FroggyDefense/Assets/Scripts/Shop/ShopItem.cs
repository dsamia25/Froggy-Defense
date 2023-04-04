using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Shop
{
    public abstract class ShopItem
    {
        public string Title = "Shop Item";
        public string Description = "Item Description";
        public int Price = 1;

        public abstract bool Buy(Character buyer);
    }
}