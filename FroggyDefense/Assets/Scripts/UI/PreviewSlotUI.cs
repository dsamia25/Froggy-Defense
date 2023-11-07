using UnityEngine;
using UnityEngine.UI;
using FroggyDefense.Core;
using FroggyDefense.Core.Spells;

public class PreviewSlotUI : MonoBehaviour
{
    [SerializeField] protected Image IconImage;

    public void Init()
    {
        UpdateUI();

        GameManager.instance.m_Player.CastSpellEvent += UpdateUI;
        GameManager.instance.m_Player.SpellDeckChangedEvent += UpdateUI;
        GameManager.instance.m_Player.SelectedSpellDeck.OnSpellDeckChanged += OnSpellDeckChangedEvent;
    }

    /// <summary>
    /// Makes sure the UI is up to date matching the correct spell.
    /// </summary>
    public void UpdateUI()
    {
        IconImage.sprite = GameManager.instance.m_Player.SelectedSpellDeck.TopSpell.Template.Icon;
    }

    private void OnSpellDeckChangedEvent(int slot, Spell spell)
    {
        UpdateUI();
    }
}
