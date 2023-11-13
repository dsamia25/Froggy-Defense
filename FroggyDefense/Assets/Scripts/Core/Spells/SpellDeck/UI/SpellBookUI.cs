using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellBookUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform listTransform;
        [SerializeField] private GameObject spellBookItemPrefab;
        [SerializeField] private ToggleButton sortingToggle;

        private Dictionary<int, SpellBookItemUI> learnedSpellIndex;
        private SpellSorter.SpellComparison sortingOrder = SpellSorter.ManaCostSort;

        private void Awake()
        {
            learnedSpellIndex = new Dictionary<int, SpellBookItemUI>();
        }

        private void Start()
        {
            UpdateUI();

            player.SpellDeckChangedEvent += UpdateUI;
            player.SelectedSpellDeck.OnSpellDeckChanged += OnSpellDeckChangedEvent;

            sortingToggle.ToggleEvent += SortMethodChanged;
        }

        /// <summary>
        /// Updates the UI to have each of the player's learned spells.
        /// </summary>
        public void UpdateUI()
        {
            List<int> learnedSpells = player.LearnedSpells.Keys.ToList();
            // TODO: Add a filter check to filter the list by (name, cost, school).

            learnedSpells = SpellSorter.SortSpellIdList(sortingOrder, learnedSpells, player);

            foreach (int spellId in learnedSpells)
            {
                // TODO: If the deck contains the spell, change the spell's color to show it. (Blue-ish tint?)

                // If already has ui element for the spell, continue.
                if (learnedSpellIndex.ContainsKey(spellId)) continue;

                // Must create a new element for the spell.
                AddLearnedSpellItem(player.GetSpellById(spellId));
            }
        }

        /// <summary>
        /// Sets the sorting method to match the button state.
        /// </summary>
        /// <param name="sorting"></param>
        public void SortMethodChanged()
        {
            switch (sortingToggle.StateIndex)
            {
                case 0:
                    sortingOrder = SpellSorter.ManaCostSort;
                    break;
                case 1:
                    sortingOrder = SpellSorter.ManaCostSortInverse;
                    break;
                case 2:
                    sortingOrder = SpellSorter.AlphabeticalSort;
                    break;
                case 3:
                    sortingOrder = SpellSorter.AlphabeticalSortInverse;
                    break;
                default:
                    sortingOrder = SpellSorter.ManaCostSort;
                    break;
            }
            UpdateUI();
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

        private void OnSpellDeckChangedEvent(int slot, Spell spell)
        {
            // TODO: Update the UI.
        }
    }
}