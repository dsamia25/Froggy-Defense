using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells.UI
{
    // TODO: Can probably just make an IUsable that Spell and items and things all can use to not have to have so many kinds of UI slots.
    public class AbilitySlotUI : MonoBehaviour
    {
        public Spell m_Spell;

        public void UseSpell()
        {
            //GameManager.instance.m_Player.m_SpellCaster.Cast(m_Spell);
            UpdateUI();
        }

        // TODO: May need to add this as a callback method for m_SpellCaster.Cast.
        public void UpdateUI()
        {
            Debug.Log("Using AbilitySlot (" + (m_Spell != null ? m_Spell.Name : "EMPTY") + ").");
        }
    }
}