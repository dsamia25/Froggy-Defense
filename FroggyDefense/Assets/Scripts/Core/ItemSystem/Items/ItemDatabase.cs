using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "ScriptableObjects/ItemSystem/Items/Item Database")]
    public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public static ItemDatabase instance;                // Singleton

        public ItemObject[] ItemList;                       // List of all items in the game. Used to assign Id's.
        public Dictionary<ItemObject, int> ItemIdIndex;     // Lookup chart of each item and their Id number.

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Debug.LogWarning($"Error: already instance of ItemDatabase.");
            }
            
            AssignIds();
        }

        /// <summary>
        /// Assigns a unique item id to all items.
        /// </summary>
        private void AssignIds()
        {
            if (ItemList == null) return;

            ItemIdIndex = new Dictionary<ItemObject, int>();

            for (int i = 0; i < ItemList.Length; i++)
            {
                ItemList[i].Id = i;
                ItemIdIndex.Add(ItemList[i], i);
            }
        }

        public void OnAfterDeserialize()
        {
            AssignIds();
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}