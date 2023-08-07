using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Spell Database", menuName = "ScriptableObjects/Spells/Spell Database")]
    public class SpellDatabase : ScriptableObject
    {
        public static SpellDatabase instance;      // Singleton

        public SpellObject[] SpellList;           // List of all actions in the game.

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning($"Error: already instance of ItemDatabase.");
            }

            AssignIds();
        }

        /// <summary>
        /// Assigns a unique item id to all actions.
        /// </summary>
        private void AssignIds()
        {
            if (SpellList == null) return;

            for (int i = 0; i < SpellList.Length; i++)
            {
                SpellList[i].SpellId = i;
            }
        }

        public void OnAfterDeserialize()
        {
            AssignIds();
        }
    }
}