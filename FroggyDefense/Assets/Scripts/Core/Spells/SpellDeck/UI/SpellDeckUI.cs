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

        private SpellDeck deck => player.SelectedSpellDeck;
        private Dictionary<int, SpellDeckItemUI> spellCardIndex;

        private void Awake()
        {
            spellCardIndex = new Dictionary<int, SpellDeckItemUI>();
        }

        private void Start()
        {
            UpdateUI();

            // TODO: Add a SpellDeck.SpellDeckChangedEvent to listen to.
            player.SpellDeckChangedEvent += UpdateUI;
        }

        /// <summary>
        /// Updates the UI to match the spell deck.
        /// </summary>
        private void UpdateUI()
        {
            // Loops through each spell in the deck.
            foreach (int spellId in deck.IncludedCards)
            {
                // If already has ui element for the spell, continue.
                if (spellCardIndex.ContainsKey(spellId)) continue;

                // Must create a new element for the spell.
                AddSpellCardItem(player.GetSpellById(spellId));
            }

            SortSpellCardItems();
        }

        /// <summary>
        /// Deletes all ui elements.
        /// </summary>
        private void Clear()
        {
            foreach (var ui in spellCardIndex.Values)
            {
                Destroy(ui.gameObject);
            }

            spellCardIndex = new Dictionary<int, SpellDeckItemUI>();
        }

        /// <summary>
        /// Sorts the spell card items to be in the right order by the graphic.
        /// </summary>
        private void SortSpellCardItems()
        {
            // TODO: Sort by: ManaCost > Alphabetical
        }

        /// <summary>
        /// Adds a spell card ui item.
        /// </summary>
        private void AddSpellCardItem(Spell spell)
        {
            if (spellCardIndex.ContainsKey(spell.SpellId))
            {
                Debug.Log($"SpellDeckUI: Deck already contains this spell ({spell.Name}, {spell.SpellId}).");
                return;
            }

            SpellDeckItemUI spellDeckItem = Instantiate(spellDeckItemPrefab, listTransform).GetComponent<SpellDeckItemUI>();
            spellCardIndex.Add(spell.SpellId, spellDeckItem);   // Store the pair in the index.
            spellDeckItem.Init(spell, RemoveSpell);     // Init spell deck item with remove callback passed in.
        }

        /// <summary>
        /// Removes a spell card ui item.
        /// </summary>
        private void RemoveSpellCardItem(Spell spell)
        {
            Debug.Log($"SpellDeckUI: Removing spell card item ui ({spell.Name}, {spell.SpellId}).");
            if (!spellCardIndex.ContainsKey(spell.SpellId))
            {
                Debug.Log($"SpellDeckUI: Deck does not contain this spell ({spell.Name}, {spell.SpellId}).");
                return;
            }

            Destroy(spellCardIndex[spell.SpellId].gameObject);
            spellCardIndex.Remove(spell.SpellId);
        }

        /// <summary>
        /// Adds a spell to the spell deck.
        /// </summary>
        private bool AddSpell(Spell spell)
        {
            // TODO: Checks if there is enough room in the deck.

            // TODO: Checks if the player knows the spell.
            // TODO: Checks if the deck already has the spell.
            // TODO: Adds the spell to the list.

            UpdateUI();

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes a spell from the spell deck.
        /// </summary>
        private bool RemoveSpell(Spell spell)
        {
            //// Checks if removing the card would bring the deck below its minimum size.
            //if (deck.DeckSize - 1 < deck.MinDeckSize)
            //{
            //    Debug.Log($"Error: Cannot remove card. Removing this card would brign the deck below its minimum size.");
            //    return false;
            //}
            bool result = deck.Remove(spell.SpellId);
            if (result)
            {
                RemoveSpellCardItem(spell);
            }
            return result;
        }
    }
}