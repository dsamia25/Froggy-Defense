using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FroggyDefense.Core.UI;

namespace FroggyDefense.Core.Spells.UI
{
    public class SpellBookUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform listTransform;
        [SerializeField] private GameObject spellBookItemPrefab;
        [SerializeField] private ToggleButton sortingToggle;

        private List<SpellBookItemUI> learnedSpellUIList;                               // The list of UI items for each learned spell.
        private Dictionary<int, SpellBookItemUI> learnedSpellIndex;

        public SpellSorter.SpellComparison SortingOrder { get; private set; } = SpellSorter.ManaCostSort;

        private void Awake()
        {
            learnedSpellUIList = new List<SpellBookItemUI>();
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

            learnedSpells = SpellSorter.SortSpellIdList(SortingOrder, learnedSpells, player);

            ResizingList<SpellBookItemUI>.Resize(spellBookItemPrefab, listTransform, learnedSpellUIList, learnedSpells.Count, 0);

            for (int i = 0; i < learnedSpellUIList.Count; i++)
            {
                learnedSpellUIList[i].Init(player.GetSpellById(learnedSpells[i]), AddSpellToDeck);
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
                    SortingOrder = SpellSorter.ManaCostSort;
                    break;
                case 1:
                    SortingOrder = SpellSorter.ManaCostSortInverse;
                    break;
                case 2:
                    SortingOrder = SpellSorter.AlphabeticalSort;
                    break;
                case 3:
                    SortingOrder = SpellSorter.AlphabeticalSortInverse;
                    break;
                default:
                    SortingOrder = SpellSorter.ManaCostSort;
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
            learnedSpell.Init(spell, AddSpellToDeck);                     // Init spell book item with remove callback passed in.
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
        private bool AddSpellToDeck(Spell spell)
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