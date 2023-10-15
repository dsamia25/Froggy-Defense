using UnityEngine;
using UnityEngine.UI;
using FroggyDefense.Core;

public class PreviewSlotUI : MonoBehaviour
{
    [SerializeField] protected Image IconImage;

    public void Init()
    {
        UpdateUI();

        GameManager.instance.m_Player.CastSpellEvent += UpdateUI;
    }

    /// <summary>
    /// Makes sure the UI is up to date matching the correct spell.
    /// </summary>
    public void UpdateUI()
    {
        IconImage.sprite = GameManager.instance.m_Player.SelectedSpellDeck.TopSpell.Template.Icon;
    }
}
