using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellDeckUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform listTransform;
        [SerializeField] private GameObject spellDeckItemPrefab;

        // Updates the shown list to match the player's spell deck.

        // Has add and remove methods for the items to call to remove spells from the spell deck. => Will update UI on spell deck changing.

        private void Start()
        {
            player.SpellDeckChangedEvent += UpdateUI;    
        }

        /// <summary>
        /// Updates the UI to match the spell deck.
        /// </summary>
        private void UpdateUI()
        {
            // TODO: Updates the list.
            // TODO: Has a way to add and remove items.
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds a spell to the spell deck.
        /// </summary>
        private bool AddSpell()
        {
            // TODO: Checks if there is enough room in the deck.
            // TODO: Checks if the player knows the spell.
            // TODO: Checks if the deck already has the spell.
            // TODO: Adds the spell to the list.
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes a spell from the spell deck.
        /// </summary>
        private bool RemoveSpell(Spell spell)
        {
            // TODO: Tries to remove the spell from the deck.
            throw new System.NotImplementedException();
        }
    }
}