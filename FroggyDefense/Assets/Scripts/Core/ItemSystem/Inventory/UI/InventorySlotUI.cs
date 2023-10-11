using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Core.Items.UI
{
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static List<Collider2D> colliderList;               // List used for checking colliders. Cached to not need to constantly allocate/deallocate.
        private static ShapeDrawer.Shape clickShape;

        [SerializeField] protected Image _iconImage;                // The item's image.
        [SerializeField] protected TextMeshProUGUI _countText;      // The text displaying the number of items in the slot.
        [SerializeField] protected Image _itemRarityBorder;         // The color-changing border to indicate an item's rarity.
        [SerializeField] protected Image _itemBorder;               // The border of the item that highlights when moused over.
        [SerializeField] protected Color _borderHighlightColor;
        [SerializeField] protected Color _borderDefaultColor;
        [SerializeField] protected LayerMask slotLayer;

        public InventoryUI HeadInventoryUi;

        protected FixedInventorySlot _slot = null;
        public FixedInventorySlot Slot
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

        private void Start()
        {
            if (colliderList == null)
            {
                colliderList = new List<Collider2D>();
            }

            if (clickShape.Equals(null))
            {
                clickShape = new ShapeDrawer.Shape(ShapeDrawer.eShape.Circle, new Vector2(.1f, .1f));
            }
        }

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
            _iconImage.transform.SetParent(HeadInventoryUi.ItemDraggingParent);
            _iconImage.transform.SetAsLastSibling();

            CloseItemDetailView();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            _iconImage.transform.position = Input.mousePosition;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"Looking for other slots:\nMouse Position {Input.mousePosition}.\nScreenPointToWorldPoint {Camera.main.ScreenToWorldPoint(Input.mousePosition)}.\nScreenPointToViewportPoint {Camera.main.ScreenToViewportPoint(Input.mousePosition)}.\nScreenPointToRay {Camera.main.ScreenPointToRay(Input.mousePosition)}");
            // Check if dragging over another slot.
            if (ActionUtils.GetTargets(Camera.main.ScreenToWorldPoint(Input.mousePosition), clickShape, slotLayer, colliderList) > 0)
            {
                Debug.Log($"Found {colliderList.Count} ui components.");
                InventorySlotUI otherSlot = null;
                for (int i = 0; i < colliderList.Count; i++)
                {
                    Debug.Log($"Looking at {colliderList[i].gameObject.name} for InventorySlotUI component.");
                    if ((otherSlot = colliderList[i].GetComponent<InventorySlotUI>()) != null)
                    {
                        FixedInventorySlot.Swap(Slot, otherSlot.Slot);
                        UpdateUI();
                        otherSlot.UpdateUI();
                        return;
                    }
                }
            }

            // Otherwise, return to the original position.
            _iconImage.transform.position = SelectedStartingPosition;
            _iconImage.transform.SetParent(transform);
            _iconImage.transform.SetAsLastSibling();
            _countText.transform.SetAsLastSibling();
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
            if (Slot.IsEmpty) return;

            HeadInventoryUi.CreateItemDetailView(this);
        }

        /// <summary>
        /// Closes the item detail view.
        /// </summary>
        public void CloseItemDetailView()
        {
            if (HeadInventoryUi == null) return;
            if (Slot == null) return;
            if (Slot.IsEmpty) return;

            HeadInventoryUi.CloseItemDetailView(Slot.item);
        }
    }
}
