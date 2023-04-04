using UnityEngine;
using TMPro;
using FroggyDefense.Shop;
using FroggyDefense.Core;

namespace FroggyDefense.UI
{
    public class StoreItemUI : MonoBehaviour, IInteractable
    {
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

            TitleText.text = _item.Title;
            DescriptionText.text = _item.Description;
            PriceText.text = PriceText.ToString();
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