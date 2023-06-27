using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Interactables;

namespace FroggyDefense.Core.Items
{
    public class ItemDropper : MonoBehaviour, IDropper
    {
        public float m_DropTimeSpacing = .1f;   // The time between each drop.
        public float m_DropForce = 1f;          // The force pushing each item when it is dropped.

        [HideInInspector]
        public List<ItemObject> Items;                    // List of items to drop.

        public void Drop()
        {
            StartCoroutine(DropSequence());
        }

        private IEnumerator DropSequence()
        {
            foreach (var item in Items)
            {

            }
            Destroy(gameObject);
            yield return null;
        }
    }
}