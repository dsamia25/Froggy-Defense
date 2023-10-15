using UnityEngine;

namespace FroggyDefense.Core.Spells
{
    [CreateAssetMenu(fileName = "New Spell Database", menuName = "ScriptableObjects/Spells/Spell Database")]
    public class SpellDatabase : ScriptableObject
    {
        public SpellObject[] SpellList;           // List of all actions in the game.

        private void Awake()
        {
            AssignIds();
        }

        /// <summary>
        /// Assigns a unique item id to all actions.
        /// </summary>
        private void AssignIds()
        {
            if (SpellList == null)
            {
                Debug.Log($"Cannot assign spell Id's. SpellList is null.");
                return;
            }

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