using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace FroggyDefense.Core.Items.UI
{
    public class ItemDetailViewUI : MonoBehaviour, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI TitleText;
        [SerializeField] private TextMeshProUGUI StatsText;
        [SerializeField] private TextMeshProUGUI DescriptionText;

        private Item _displayedItem;
        public Item DisplayedItem
        {
            get => _displayedItem;
            set
            {
                _displayedItem = value;
                UpdateUI();
            }
        }

        public delegate void ItemDetailViewDelegate(Item item);
        public static event ItemDetailViewDelegate MouseExitEvent;

        /// <summary>
        /// Opens the detail view.
        /// </summary>
        public void Open(Item item)
        {
            DisplayedItem = item;
            
            if (_displayedItem != null) {
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Closes the detail view.
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"Pointer exitting ItemDetailView.");
            MouseExitEvent?.Invoke(_displayedItem);
            Close();
        }

        /// <summary>
        /// Update UI components.
        /// </summary>
        public void UpdateUI()
        {
            try
            {
                // Set text.
                TitleText.text = _displayedItem.Name;
                DescriptionText.text = _displayedItem.Description;

                // Set text color.
                TitleText.color = GameManager.instance.m_UiManager.ItemRarityColors.GetColor(_displayedItem.Rarity);
            } catch (Exception e) {
                Debug.LogWarning($"Error loading detail view: {e}");
            }
        }
    }
}