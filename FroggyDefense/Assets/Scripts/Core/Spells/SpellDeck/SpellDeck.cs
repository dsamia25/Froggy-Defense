using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [Serializable]
    public class SpellDeck
    {
        [SerializeField] private List<int> includedCards;
        [SerializeField] private Queue<Spell> Deck;
        [SerializeField] private Spell[] Hand;

        private int minDeckSize = 8;
        private int maxDeckSize = 12;
        private int maxHandSize = 4;

        public IReadOnlyList<int> IncludedCards => includedCards.AsReadOnly();
        public Spell TopSpell => Deck.Peek();
        public bool IsValidDeck => includedCards.Count >= minDeckSize;
        public int DeckSize => Deck.Count;
        public bool DeckEmpty => Deck.Count <= 0;
        public int HandSize => maxHandSize;
        public int MinDeckSize => minDeckSize;
        public int MaxDeckSize => maxDeckSize;

        public delegate void SpellDeckDelegate(int slot, Spell spell);
        public event SpellDeckDelegate OnSpellCardDrawn;
        public event SpellDeckDelegate OnHandSizeChanged;
        public event SpellDeckDelegate OnSpellDeckChanged;

        public SpellDeck()
        {
            Deck = new Queue<Spell>();
            Hand = new Spell[maxHandSize];
            includedCards = new List<int>();
        }

        /// <summary>
        /// Adds a new spell to the deck.
        /// </summary>
        /// <returns></returns>
        public bool Add(Spell spell)
        {
            Debug.Log($"SpellDeck: Adding {spell.Name} to Spell Deck.");
            if (includedCards.Contains(spell.SpellId) || includedCards.Count > maxDeckSize)
            {
                return false;
            }

            includedCards.Add(spell.SpellId);
            Deck.Enqueue(spell);

            Draw();

            OnSpellDeckChanged?.Invoke(-1, null);

            return true;
        }

        /// <summary>
        /// Removes a spell from the SpellDeck.
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public bool Remove(int spellId)
        {
            Debug.Log($"SpellDeck: Removing spell ({spellId}).");
            if (!includedCards.Contains(spellId)) return false;

            // Check if the card is in the hand.
            for (int i = 0; i < Hand.Length; i++)
            {
                if (Hand[i] == null) continue;
                if (Hand[i].SpellId == spellId)
                {
                    Hand[i] = null;
                    includedCards.Remove(spellId);
                    Draw();
                    OnSpellDeckChanged?.Invoke(-1, null);
                    return true;
                }
            }

            // Check if the card is in the deck. Remove the card by creating a new queue without the selected spell.
            bool wasRemoved = false;
            Queue<Spell> newDeck = new Queue<Spell>();
            for (int i = 0; i < Deck.Count; i++)
            {
                Spell spellCard = Deck.Dequeue();
                if (spellCard.SpellId == spellId)
                {
                    wasRemoved = true;
                    includedCards.Remove(spellId);
                    continue;
                }
                newDeck.Enqueue(spellCard);
            }

            // Save the new deck without the removed spell.
            Deck = newDeck;

            // Return whether it was removed. (Should be true at this point unless somethign went wrong somehow).
            OnSpellDeckChanged?.Invoke(-1, null);
            return wasRemoved;
        }

        public Spell GetSpell(int slot)
        {
            if (slot < 0 || slot >= maxHandSize) return null;
            return Hand[slot];
        }

        /// <summary>
        /// Tries to draw a card into an empty slot.
        /// </summary>
        public void Draw()
        {
            if (DeckEmpty) return;

            for (int i = 0; i < maxHandSize; i++)
            {
                if (Hand[i] == null)
                {
                    Hand[i] = Deck.Dequeue();
                    OnSpellCardDrawn?.Invoke(i, Hand[i]);
                    Debug.Log($"SpellDeck: Card drawn (Slot {i}).");
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the spell at the slot to the deck. Draws a new card.
        /// </summary>
        /// <param name="slot"></param>
        public void Return(int slot)
        {
            if (slot < 0 || slot >= maxHandSize) return;

            Deck.Enqueue(Hand[slot]);
            Hand[slot] = null;

            Draw();
        }
    }
}