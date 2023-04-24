using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Shop
{
    public class TravelingShop : MonoBehaviour, IInteractable
    {
        public bool ShopIsOpen = true;                                              // If the shop is open an interactable.
        public int ShopSize { get => _items.Count; }                                // How many items are in the shop.
        public ItemObject[] _itemInventoryTemplates;                                // The list of inventory items to sell.

        private List<ShopItem> _items = new List<ShopItem>();                       // List of all items in the shop.
        public IReadOnlyCollection<ShopItem> Items { get => _items.AsReadOnly(); }  // Returns the items in the shop as a readonly collection.

        [Space]
        [Header("Interact Events")]
        [Space]
        public UnityEvent InteractEvent;

        // TODO: Convert the ItemObject list to a ShopItemObject list that can hold shop info such as limited amounts and prices.
        private void Awake()
        {
            GenerateShopInventory();
        }

        /// <summary>
        /// Converts the ItemObjects in template list to actual items.
        /// </summary>
        public void GenerateShopInventory()
        {
            foreach (ItemObject obj in _itemInventoryTemplates)
            {
                ShopItem newItem = new ShopItem(Item.CreateItem(obj));
                _items.Add(newItem);
            }
        }

        /// <summary>
        /// Gets the item at the given index from the shop.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ShopItem GetShopItem(int index)
        {
            if (_items.Count == 0) return null;

            return _items[index % _items.Count];
        }

        /// <summary>
        /// Attempts to buy the item from the shop. Removes the item if successful.
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Buy(Character buyer, ShopItem item)
        {
            if (GameManager.instance.m_GemManager.Gems >= item.Price)
            {
                if (item.Buy(buyer))
                {
                    // Removes the item from the store if there is a limited amount.
                    if (item.LimitedAmount && item.Amount <= 0)
                    {
                        Remove(item);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the item from the shop and destorys its UI elements.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(ShopItem item)
        {
            _items.Remove(item);
        }

        /// <summary>
        /// Interact with the object.
        /// </summary>
        public void Interact()
        {
            InteractEvent?.Invoke();
        }

        /// <summary>
        /// OnClick.
        /// </summary>
        public void OnMouseUpAsButton()
        {
            // Click on the Nexus to interact with it.
            if (ShopIsOpen)
            {
                Interact();
            }
        }

    }
}