using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Action Database", menuName = "ScriptableObjects/Actions/Action Database")]
    public class ActionDatabase : ScriptableObject
    {
        public ActionObject[] ActionList;           // List of all actions in the game.

        private void Awake()
        {
            AssignIds();
        }

        /// <summary>
        /// Assigns a unique item id to all actions.
        /// </summary>
        private void AssignIds()
        {
            if (ActionList == null)
            {
                Debug.Log($"Cannot assign action Id's. ActionList is null.");
                return;
            }

            for (int i = 0; i < ActionList.Length; i++)
            {
                ActionList[i].ActionId = i;
            }
        }

        //public void OnAfterDeserialize()
        //{
        //    AssignIds();
        //}
    }
}