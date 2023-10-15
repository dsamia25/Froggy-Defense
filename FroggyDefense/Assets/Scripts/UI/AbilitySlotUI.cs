using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FroggyDefense.Core.Spells.UI
{
    public class AbilitySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image _iconImage;                // The item's image.
        [SerializeField] protected TextMeshProUGUI _hotkey;         // The text displaying the button to press to use the ability.
        [SerializeField] protected Image _cooldownFill;
        [SerializeField] protected TextMeshProUGUI _cooldownText;   // The text displaying the button to press to use the ability.

        [SerializeField] protected Image _itemBorder;               // The border of the item that highlights when moused over.
        [SerializeField] protected Color _borderHighlightColor;
        [SerializeField] protected Color _borderDefaultColor;

        [SerializeField] private Spell _spell;

        private int slotNum = 0;
        public int SlotNum { get => slotNum; set { slotNum = value; _hotkey.text = $"{SlotNum}";} }
        public Spell SelectedSpell
        {
            get => _spell;
            set
            {
                _spell = value;
                UpdateUI();
            }
        }

        private InputController controller;

        //public event EventHandler ClickedEvent;

        public void Init(int slotNum)
        {
            SlotNum = slotNum;

            controller = GameManager.instance.m_Player.GetComponent<InputController>();

            UpdateUI();

            GameManager.instance.m_Player.CastSpellEvent += UpdateUI;
        }

        //private void Update()
        //{
        //    if (SelectedSpell != null && SelectedSpell.OnCooldown)
        //    {
        //        float cooldown = SelectedSpell.CurrCooldown / SelectedSpell.Cooldown;
        //        _cooldownFill.fillAmount = cooldown;
        //        _cooldownText.text = cooldown.ToString("0.0");
        //        _cooldownFill.gameObject.SetActive(true);
        //        _cooldownText?.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        _cooldownFill?.gameObject.SetActive(false);
        //        _cooldownText?.gameObject.SetActive(false);
        //    }
        //}

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Change the highlight border to the highlight color.
            _itemBorder.color = _borderHighlightColor;

            //OpenItemDetailView();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Change the highlight border to the default color.
            _itemBorder.color = _borderDefaultColor;

            //CloseItemDetailView();
        }

        /// <summary>
        /// On use effect.
        /// </summary>
        public void UseSpell()
        {
            controller.UseAbility(SlotNum);
        }

        /// <summary>
        /// Makes sure the UI is up to date matching the correct spell.
        /// </summary>
        public void UpdateUI()
        {
            _spell = GameManager.instance.m_Player.GetSpell(slotNum);

            if (SelectedSpell != null)
            {
                _iconImage.sprite = SelectedSpell.Template.Icon;
                _iconImage.gameObject.SetActive(true);
                _cooldownFill.gameObject.SetActive(true);
            }
            else
            {
                _iconImage.gameObject.SetActive(false);
                _cooldownFill.gameObject.SetActive(false);
            }
        }
    }
}