using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FroggyDefense.Core;

namespace FroggyDefense.Shop.UI
{
    public class ShopItemUI : MonoBehaviour, IInteractable
    {
        public Image Icon; 
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI DescriptionText;
        public TextMeshProUGUI PriceText;

        public TravelingShop Shop;  // The shop this item is in.

        private ShopItem _item = null;
        public ShopItem Item
        {
            get => _item;
            set
            {
                _item = value;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (_item == null) return;

            if (_item.Icon == null)
            {
                Icon.gameObject.SetActive(false);
            } else
            {
                Icon.sprite = _item.Icon;
                Icon.gameObject.SetActive(true);
            }
            TitleText.text = _item.Title;
            DescriptionText.text = _item.Description;
            PriceText.text = _item.Price.ToString();
            Debug.Log("ShopItemUI for " + _item.Title + " = " + _item.Price.ToString() + " gems.");
        }

        /// <summary>
        /// Attempts to buy the item.
        /// </summary>
        /// <returns></returns>
        private bool Buy()
        {
            // TODO: Maybe make something for limited amount items.
            return Shop.Buy(GameManager.instance.m_Player, Item);
        }

        public void Interact()
        {
            Buy();
        }
    }
}