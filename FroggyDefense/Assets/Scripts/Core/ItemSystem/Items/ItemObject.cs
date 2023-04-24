using UnityEngine;
using FroggyDefense.Economy;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Items/Item")]
    public class ItemObject : ScriptableObject
    {
        [Space]
        [Header("Properties")]
        [Space]
        public string Name = "ITEM";
        public string Description = "A NEW ITEM";
        public bool IsStackable { get; } = false;
        public ItemType Type = ItemType.Default;

        public Sprite Icon = null;
        public CurrencyObject PriceCurrency;
        public int ShopPrice = 1;
    }
}