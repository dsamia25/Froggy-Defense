using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace FroggyDefense.Core.Items.UI
{
    public class ItemDetailViewUI : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI TitleText;
        [SerializeField] protected TextMeshProUGUI StatsText;
        [SerializeField] protected TextMeshProUGUI DescriptionText;
        [SerializeField] protected Image _itemRarityBorder;             // The color-changing border to indicate an item's rarity.

        [SerializeField] protected float _moveDelay = .1f;              // How long the item has held over before being delayed.

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
        public static event ItemDetailViewDelegate UpdatedEvent;

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

                // Set colors.
                Color color = GameManager.instance.m_UiManager.ItemRarityColors.GetColor(_displayedItem.Rarity);
                TitleText.color = color;
                _itemRarityBorder.color = color;

                StartCoroutine(WaitForMoveDelay(_moveDelay));
            } catch (Exception e) {
                Debug.LogWarning($"Error loading detail view: {e}");
            }
        }

        private IEnumerator WaitForMoveDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UpdatedEvent?.Invoke(_displayedItem);
        }
    }
}