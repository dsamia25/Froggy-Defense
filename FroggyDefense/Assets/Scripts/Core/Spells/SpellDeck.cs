using System;
using System.Collections.Generic;

namespace FroggyDefense.Core.Spells
{
    public class SpellDeck
    {
        private List<SpellCard> deck;
        private List<SpellCard> hand;

        private int minDeckSize = 8;
        private int maxDeckSize = 12;
        private int handSize = 4;

        public SpellDeck()
        {
            deck = new List<SpellCard>();
            hand = new List<SpellCard>();
        }

        /// <summary>
        /// Adds a new spell to the deck
        /// </summary>
        /// <returns></returns>
        public bool Add(Spell spell)
        {
            throw new NotImplementedException();
        }

        // An individual spell card. Keeps track of a spell and how many charges it has.
        public class SpellCard
        {
            public Spell Spell { get; private set; }
            public int MaxCharges { get; private set; }
            public int Charges { get; private set; }

            public SpellCard(Spell spell, int charges)
            {
                Spell = spell;
                MaxCharges = charges;
                Charges = charges;
            }

            public void Refresh()
            {
                Charges = MaxCharges;
            }
        }
    }
}