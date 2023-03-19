using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using FroggyDefense.Items;

namespace FroggyDefense.UI
{
    public class ItemButtonUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _iconImage;             // The item's image.
        [SerializeField] private TextMeshProUGUI _countText;   // The text displaying the number of items in the slot.
        public InventoryUI HeadInventoryUi;

        private InventorySlot _slot = null;
        public InventorySlot Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                UpdateUI();
            }
        }       // The inventory slot this icon is representing.

        private Vector3 SelectedStartingPosition = Vector3.zero;
        private bool _clickedDownOnButton = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            _clickedDownOnButton = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Slot == null || Slot.IsEmpty)
            {
                return;
            }

            if (_clickedDownOnButton)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Slot.UseItem();
                    UpdateUI();
                }
            }
            _clickedDownOnButton = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SelectedStartingPosition = transform.position;
            _iconImage.transform.SetParent(transform.parent.parent);
            _iconImage.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _iconImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _iconImage.transform.position = SelectedStartingPosition;
            _iconImage.transform.SetParent(transform);
            _iconImage.transform.SetAsLastSibling();
            Debug.Log("Released mouse over button.");
        }

        /// <summary>
        /// Updates the UI elements of the icon.
        /// </summary>
        public void UpdateUI()
        {
            if (Slot == null || Slot.IsEmpty)
            {
                _iconImage.gameObject.SetActive(false);
                _countText.gameObject.SetActive(false);
                return;
            }

            // Update the sprite.
            _iconImage.sprite = Slot.item.Icon;
            _iconImage.gameObject.SetActive(true);

            // Only display the count if stackable.
            _countText.text = Slot.count.ToString();
            _countText.gameObject.SetActive(Slot.item.IsStackable);
        }
    }
}
