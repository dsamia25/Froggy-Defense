using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FroggyDefense.Core.Items.UI
{
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {        
        [SerializeField] protected Image _iconImage;                // The item's image.
        [SerializeField] protected TextMeshProUGUI _countText;      // The text displaying the number of items in the slot.
        [SerializeField] protected Image _itemRarityBorder;         // The color-changing border to indicate an item's rarity.
        [SerializeField] protected Image _itemBorder;               // The border of the item that highlights when moused over.
        [SerializeField] protected Color _borderHighlightColor;
        [SerializeField] protected Color _borderDefaultColor;

        public InventoryUI HeadInventoryUi;

        protected InventorySlot _slot = null;
        public InventorySlot Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                UpdateUI();
            }
        }       // The inventory slot this icon is representing.

        protected Vector3 SelectedStartingPosition = Vector3.zero;
        protected bool _clickedDownOnButton = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Change the highlight border to the highlight color.
            _itemBorder.color = _borderHighlightColor;

            OpenItemDetailView();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Change the highlight border to the default color.
            _itemBorder.color = _borderDefaultColor;

            CloseItemDetailView();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _clickedDownOnButton = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (Slot == null || Slot.IsEmpty)
            {
                return;
            }

            if (_clickedDownOnButton)
            {
                // Listen for right click.
                if (Input.GetMouseButtonUp(1))
                {
                    Slot.UseItem();
                    UpdateUI();
                }
            }
            _clickedDownOnButton = false;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            SelectedStartingPosition = transform.position;
            _iconImage.transform.SetParent(transform.parent.parent);
            _iconImage.transform.SetAsLastSibling();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            _iconImage.transform.position = Input.mousePosition;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _iconImage.transform.position = SelectedStartingPosition;
            _iconImage.transform.SetParent(transform);
            _iconImage.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Updates the UI elements of the icon.
        /// </summary>
        public virtual void UpdateUI()
        {
            try
            {
                if (Slot == null || Slot.IsEmpty)
                {
                    _itemRarityBorder.color = _borderDefaultColor;
                    _iconImage.gameObject.SetActive(false);
                    _countText.gameObject.SetActive(false);
                    return;
                }

                // Update the sprite.
                _iconImage.sprite = Slot.item.Icon;
                _iconImage.gameObject.SetActive(true);

                // Update the rarity color.
                _itemRarityBorder.color = GameManager.instance.m_UiManager.ItemRarityColors.GetColor(Slot.item.Rarity);

                // Only display the count if stackable.
                _countText.text = Slot.count.ToString();
                _countText.gameObject.SetActive(Slot.item.IsStackable);
            } catch (Exception e)
            {
                Debug.Log($"Error updating inventory slot ui: {e}");
            }
        }

        /// <summary>
        /// Opens the detail view showing the item name, stats, and use effects.
        /// </summary>
        public void OpenItemDetailView()
        {
            if (HeadInventoryUi == null) return;
            if (Slot == null) return;

            HeadInventoryUi.CreateItemDetailView(this);
        }

        /// <summary>
        /// Closes the item detail view.
        /// </summary>
        public void CloseItemDetailView()
        {
            if (HeadInventoryUi == null) return;
            if (Slot == null) return;

            HeadInventoryUi.CloseItemDetailView(Slot.item);
        }
    }
}
