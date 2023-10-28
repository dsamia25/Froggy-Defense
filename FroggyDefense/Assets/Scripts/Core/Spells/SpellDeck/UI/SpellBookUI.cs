using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellBookUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform listTransform;
        [SerializeField] private GameObject spellBookItemUI;

        private Dictionary<int, SpellBookItemUI> learnedSpellIndex;

        private void Awake()
        {
            learnedSpellIndex = new Dictionary<int, SpellBookItemUI>();
        }

        private void Start()
        {
            UpdateUI();

            // TODO: Add a SpellDeck.SpellDeckChangedEvent to listen to.
            player.SpellDeckChangedEvent += UpdateUI;
        }

        /// <summary>
        /// Updates the UI to have each of the player's learned spells.
        /// </summary>
        private void UpdateUI()
        {
            foreach (Spell spell in player.LearnedSpells.Values)
            {
                //spell.
            }
        }

        /// <summary>
        /// Adds a learned spell item to the list.
        /// </summary>
        private void AddLearnedSpellItem()
        {

        }

        /// <summary>
        /// Removes a learned spell item from the list.
        /// </summary>
        private void RemoveLearnedSpellItem()
        {

        }
    }
}