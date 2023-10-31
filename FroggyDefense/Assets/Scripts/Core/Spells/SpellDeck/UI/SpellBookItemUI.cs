using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellBookItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI SpellTypeText;
        [SerializeField] private TextMeshProUGUI ManaCostText;

        private Spell spellCard;
        public Spell SpellCard {
            get => spellCard;
            set {
                if (value != null) {
                    spellCard = value;
                    UpdateUI();
                }
            }
        }

        public delegate bool SpellBookItemDelegate(Spell spell);
        private SpellBookItemDelegate AddSpellDelegate;

        /// <summary>
        /// Inits the values to the spell and sets the callback mehtods.
        /// </summary>
        /// <param name="_spellCard"></param>
        /// <param name="_callback"></param>
        public void Init(Spell _spellCard, SpellBookItemDelegate _callback) 
        {
            SpellCard = _spellCard;
            AddSpellDelegate = _callback;
        }

        /// <summary>
        /// Updates the UI to match the new spell.
        /// </summary>
        private void UpdateUI() {
            Icon.sprite = spellCard.Template.Icon;
            NameText.text = spellCard.Name;
            SpellTypeText.text = spellCard.Template.School.ToString();
            ManaCostText.text = spellCard.ManaCost.ToString();
        }

        /// <summary>
        /// Call the callback delegate when clicked.
        /// </summary>
        /// <returns></returns>
        public bool OnClick()
        {
            if (AddSpellDelegate == null) return false;

            return AddSpellDelegate.Invoke(spellCard);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"SpellBookItemUI: Clicked on item!");
            OnClick();
        }
    }
}