using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FroggyDefense.Core;
using FroggyDefense.Shop;

namespace FroggyDefense.UI
{
    public class ShopItemUI : MonoBehaviour, IInteractable
    {
        public Image Icon; 
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI DescriptionText;
        public TextMeshProUGUI PriceText;

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
        }

        private bool Buy()
        {
            return false;
        }

        public void Interact()
        {
            Buy();
        }
    }
}