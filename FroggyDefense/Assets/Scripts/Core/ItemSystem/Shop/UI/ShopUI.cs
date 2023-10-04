using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Shop.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TravelingShop _shop;

        public GameObject _storeItemUiPrefab = null;
        public Transform _storeItemUiParent = null;

        private Dictionary<ShopItem, ShopItemUI> _itemUiIndex = new Dictionary<ShopItem, ShopItemUI>(); // Each item in the shop corresponding to its UI element.

        private void Awake()
        {
            if (_storeItemUiParent == null) _storeItemUiParent = transform;
        }

        private void Start()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_shop.ShopSize <= 0)
            {
                // TODO: Make a special SHOP EMPTY slide to display.
                return;
            }

            foreach(ShopItem item in _shop.Items)
            {
                if (!_itemUiIndex.ContainsKey(item))
                {
                    var itemUi = Instantiate(_storeItemUiPrefab, _storeItemUiParent).GetComponent<ShopItemUI>();
                    itemUi.Item = item;
                    itemUi.Shop = _shop;
                    _itemUiIndex.Add(item, itemUi);
                }
            }
        }

        /// <summary>
        /// Opens up the shop window and updates its Ui elements.
        /// </summary>
        public void OpenShop()
        {
            UpdateUI();
        }
    }
}