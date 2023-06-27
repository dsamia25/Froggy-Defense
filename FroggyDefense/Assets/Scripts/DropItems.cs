using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Interactables
{
    public class DropItems : MonoBehaviour
    {
        [Space]
        [Header("Loot Table")]
        [Space]
        public LootTable Table;

        public delegate void DropItemsDelegate(DropItemsEventArgs args);
        public static DropItemsDelegate DropItemsEvent;

        /// <summary>
        /// Invokes the DropItemsEvent for a manager to spawn in a ItemDropper.
        /// </summary>
        public void Drop()
        {
            List<ItemObject> loot = Table.Roll();
            DropItemsEvent?.Invoke(new DropItemsEventArgs(transform.position, loot));
        }
    }

    public class DropItemsEventArgs : EventArgs
    {
        public Vector2 Pos;     // The position of the event.
        public List<ItemObject> Items;

        public DropItemsEventArgs(Vector2 pos, List<ItemObject> items)
        {
            Pos = pos;
            Items = items;
        }
    }
}