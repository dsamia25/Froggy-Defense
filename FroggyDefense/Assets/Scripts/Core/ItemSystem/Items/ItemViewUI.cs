using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.Core.Items.UI
{
    public class ItemViewUI : MonoBehaviour
    {
        [SerializeField] protected Image ItemIcon;                      // The item image.
        [SerializeField] protected TextMeshProUGUI TitleText;           // Title header text.
        [SerializeField] protected Image TitleBackground;               // The colored background for the title.
        [SerializeField] protected TextMeshProUGUI DetailText;          // Detail text.
        [SerializeField] protected TextMeshProUGUI DescriptionText;     // Yellow description text.
        [SerializeField] protected Image _itemRarityBorder;             // The color-changing border to indicate an item's rarity.
        [SerializeField] protected float _moveDelay = .1f;              // How long the item has held over before being delayed.

        [SerializeField] protected Transform StatRowParent;
        [SerializeField] protected List<StatRowUI> StatRows;            // Array of all stat rows
        [SerializeField] protected GameObject StatRowPrefab;
        
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

        public delegate void ItemViewUIDelegate(Item item);
        public static event ItemViewUIDelegate UpdatedEvent;

        /// <summary>
        /// Opens the detail view.
        /// </summary>
        public void Open(Item item)
        {
            DisplayedItem = item;

            if (_displayedItem != null)
            {
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
        /// Manages the stat rows to have the correct displayed values.
        /// </summary>
        private void UpdateStatRows()
        {
            if (_displayedItem.Type == ItemType.Equipment)
            {
                StatRowParent.gameObject.SetActive(true);
                //var newStatRow = Instantiate(StatRowPrefab, StatRowParent).GetComponent<StatRowUI>();
                //StatRows.Add(newStatRow);

                List<StatValuePair> stats = ((Equipment)_displayedItem).Stats;
                if (StatRows.Count < stats.Count)
                {
                    // Add new rows until same amount.
                    for (int i = StatRows.Count; i < stats.Count; i++)
                    {
                        var newStatRow = Instantiate(StatRowPrefab, StatRowParent).GetComponent<StatRowUI>();
                        StatRows.Add(newStatRow);
                    }
                }
                else if (StatRows.Count > stats.Count)
                {
                    // Remove rows until same amount.
                    for (int i = StatRows.Count; i > stats.Count; i--)
                    {
                        Destroy(StatRows[i]);
                    }
                }

                // Update the row values.
                for (int i = 0; i < stats.Count; i++)
                {
                    StatValuePair stat = stats[i];
                    StatRows[i].SetStatRow(stat.Stat.ToString().Replace('_', ' '), Mathf.FloorToInt(stat.Value));
                }
            }
            else
            {
                StatRowParent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Updates the detail text.
        /// </summary>
        private void UpdateDetailText()
        {
            if (_displayedItem.Type != ItemType.Equipment)
            {
                DetailText.text = _displayedItem.GetDetailText();
            }
        }

        /// <summary>
        /// Update UI components.
        /// </summary>
        public void UpdateUI()
        {
            try
            {
                // Set image and text.
                ItemIcon.sprite = _displayedItem.Icon;
                TitleText.text = _displayedItem.Name;
                DescriptionText.text = _displayedItem.Description;

                UpdateStatRows();
                UpdateDetailText();

                // Set colors.
                Color color = GameManager.instance.m_UiManager.ItemRarityColors.GetColor(_displayedItem.Rarity);
                //TitleText.color = color;
                TitleBackground.color = color;
                _itemRarityBorder.color = color;

                StartCoroutine(WaitForMoveDelay(_moveDelay));
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error loading detail view: {e}");
                Close();
            }
        }

        private IEnumerator WaitForMoveDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UpdatedEvent?.Invoke(_displayedItem);
        }
    }
}
