using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellDeckItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI ManaCostText;

        private bool _clickedDownOnButton = false;

        private Spell spellCard;
        public Spell SpellCard
        {
            get => spellCard;
            set {
                if (value != null)
                {
                    spellCard = value;
                    UpdateUI();
                }
            }
        }
        public delegate bool SpellDeckItemDelegate(Spell spell);
        private SpellDeckItemDelegate RemoveItemDelegate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_spellCard"></param>
        /// <param name="_callback"></param>
        public void Init(Spell _spellCard, SpellDeckItemDelegate _callback)
        {
            SpellCard = _spellCard;
            RemoveItemDelegate = _callback;
        }

        /// <summary>
        /// Updates the UI to match the new spell.
        /// </summary>
        public void UpdateUI()
        {
            Icon.sprite = spellCard.Template.Icon;
            NameText.text = spellCard.Name;
            ManaCostText.text = spellCard.ManaCost.ToString("0");
        }

        /// <summary>
        /// Call the callback delegate when clicked.
        /// </summary>
        /// <returns></returns>
        public bool OnClick()
        {
            if (RemoveItemDelegate == null) return false;

            return RemoveItemDelegate.Invoke(spellCard);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"SpellDeckItemUI: Clicked on item!");
            OnClick();
            //_clickedDownOnButton = false;
        }

        // TODO: Has onHover to show detail view for spell info.
    }
}