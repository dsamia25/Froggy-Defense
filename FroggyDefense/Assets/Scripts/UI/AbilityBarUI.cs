using UnityEngine;
using FroggyDefense.Core.Spells;
using FroggyDefense.Core.Spells.UI;

namespace FroggyDefense.Core.UI
{
    public class AbilityBarUI : MonoBehaviour
    {
        [SerializeField] private AbilitySlotUI[] AbilitySlots;      // The slots displaying the player's available spells.
        [SerializeField] private PreviewSlotUI PreviewSlot;         // The slot previewing the next spell the player will draw.
        [SerializeField] private Player player;

        private void Start()
        {
            if (player == null) player = GameManager.instance.m_Player;

            for (int i = 0; i < AbilitySlots.Length; i++)
            {
                AbilitySlots[i].Init(i);
            }

            PreviewSlot.Init();
        }
    }
}