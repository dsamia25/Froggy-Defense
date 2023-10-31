using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellBookUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform listTransform;
        [SerializeField] private GameObject spellBookItemPrefab;

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
            foreach (int spellId in player.LearnedSpells.Keys)
            {
                // If already has ui element for the spell, continue.
                if (learnedSpellIndex.ContainsKey(spellId)) continue;

                // Must create a new element for the spell.
                AddLearnedSpellItem(player.GetSpellById(spellId));
            }

            SortSpellBookItems();
        }

        // TODO: Sorts the spell book items.
        private void SortSpellBookItems()
        {
            
        }

        /// <summary>
        /// Adds a learned spell item to the list.
        /// </summary>
        private void AddLearnedSpellItem(Spell spell)
        {
            if (learnedSpellIndex.ContainsKey(spell.SpellId))
            {
                Debug.Log($"SpellDeckUI: Deck already contains this spell ({spell.Name}, {spell.SpellId}).");
                return;
            }

            SpellBookItemUI learnedSpell = Instantiate(spellBookItemPrefab, listTransform).GetComponent<SpellBookItemUI>();
            learnedSpellIndex.Add(spell.SpellId, learnedSpell);     // Store the pair in the index.
            learnedSpell.Init(spell, AddSpell);                     // Init spell book item with remove callback passed in.
        }

        /// <summary>
        /// Removes a learned spell item from the list.
        /// </summary>
        private void RemoveLearnedSpellItem(Spell spell)
        {
            Debug.Log($"SpellBookUI: Removing learned spell item ui ({spell.Name}, {spell.SpellId}).");
            if (!learnedSpellIndex.ContainsKey(spell.SpellId))
            {
                Debug.Log($"SpellBookUI: SpellBook does not contain this spell ({spell.Name}, {spell.SpellId}).");
                return;
            }

            Destroy(learnedSpellIndex[spell.SpellId].gameObject);
            learnedSpellIndex.Remove(spell.SpellId);
        }

        /// <summary>
        /// Adds a spell to the spell deck.
        /// </summary>
        private bool AddSpell(Spell spell)
        {
            // Checks if able to add to the deck.
            SpellDeck deck = player.SelectedSpellDeck;
            if (deck.DeckSize >= deck.MaxDeckSize)
            {
                Debug.Log($"Error: Cannot add any more cards.");
                return false;
            }
            return deck.Add(spell);
        }
    }
}