using System;
using System.Collections;
using UnityEngine;
using FroggyDefense.Interactables;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Core
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private GameObject GroundItemPrefab;

        public float m_DropTimeSpacing = .1f;   // The time between each drop.
        public float m_DropForce = 1f;          // The force pushing each item when it is dropped.

        private void Start()
        {
            // Subscribe to events.
            DropItems.DropItemsEvent += OnDropItemsEvent;
        }

        /// <summary>
        /// Listens for a drop item event and starts a new item drop coroutine.
        /// </summary>
        /// <param name="args"></param>
        private void OnDropItemsEvent(DropItemsEventArgs args)
        {
            try
            {
                StartCoroutine(DropItemsSequence(args));
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating ItemDropper: {e}");
            }
        }

        /// <summary>
        /// Cycles through each item in the drop list and spawns them in.
        /// Launces the items a tiny bit.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerator DropItemsSequence(DropItemsEventArgs args)
        {
            foreach (var itemObject in args.Items)
            {
                Item item = Item.CreateItem(itemObject);
                GroundItem groundItem = Instantiate(GroundItemPrefab, args.Pos, Quaternion.identity).GetComponent<GroundItem>();
                groundItem.SetItem(item);
                groundItem.Launch(m_DropForce * GetRandomAngle());

                yield return new WaitForSeconds(m_DropTimeSpacing);
            }
            yield return null;
        }

        private Vector2 GetRandomAngle()
        {
            return UnityEngine.Random.insideUnitCircle.normalized;
        }
    }
}