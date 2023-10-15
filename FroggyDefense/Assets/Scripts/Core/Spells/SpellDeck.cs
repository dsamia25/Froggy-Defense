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
        public bool ValidDeck => includedCards.Count >= minDeckSize;
        public int DeckSize => Deck.Count;
        public int HandSize => maxHandSize;

        public delegate void SpellDeckDelegate(int slot, Spell spell);
        public event SpellDeckDelegate OnSpellCardDrawn;
        public event SpellDeckDelegate OnHandSizeChanged;

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
            Debug.Log($"Adding {spell.Name} to Spell Deck.");
            if (includedCards.Contains(spell.SpellId) || includedCards.Count > maxDeckSize)
            {
                return false;
            }

            includedCards.Add(spell.SpellId);
            Deck.Enqueue(spell);

            Draw();

            return true;
        }

        ///// <summary>
        ///// Removes a spell from the SpellDeck.
        ///// </summary>
        ///// <param name="spellId"></param>
        ///// <returns></returns>
        //public bool Remove(int spellId)
        //{
        //    var deckArray = Deck.ToArray();
        //    for (int i = 0; i < deckArray.Length; i++)
        //    {
        //        if (deckArray)
        //    }
        //    return false;
        //}

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
            for (int i = 0; i < maxHandSize; i++)
            {
                if (Hand[i] == null)
                {
                    Hand[i] = Deck.Dequeue();
                    OnSpellCardDrawn?.Invoke(i, Hand[i]);
                    Debug.Log($"Card drawn (Slot {i}).");
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

    public class SpellHandSlot
    {
        public Spell Spell { get; internal set; }

        public SpellHandSlot() { }
    }
}