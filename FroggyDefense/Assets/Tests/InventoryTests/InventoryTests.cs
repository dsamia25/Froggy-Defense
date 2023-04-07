using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Tests
{
    public class InventoryTests
    {
        [UnityTest]
        public IEnumerator AddSingleTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();

            inventory.Add(item, 1);

            yield return null;

            Assert.True(inventory.Contains(item));
        }

        [UnityTest]
        public IEnumerator AddAmountTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(item, testCount);
            inventory.Add(stackableItem, testCount);

            yield return null;

            // Unstackable items always have a value of 1.
            Assert.AreEqual(1, inventory.GetCount(item));

            // Stackable items can have an amount.
            Assert.AreEqual(testCount, inventory.GetCount(stackableItem));
        }

        [UnityTest]
        public IEnumerator AddToExistingStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(item, testCount);
            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);

            yield return null;

            // Unstackable items always have a value of 1.
            Assert.AreEqual(1, inventory.GetCount(item));

            // Stackable items can have an amount.
            Assert.AreEqual(3 * testCount, inventory.GetCount(stackableItem));
        }

        [UnityTest]
        public IEnumerator SubtractNegativeStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, -1);

            yield return null;

            // Nothing removed.
            Assert.AreEqual(testCount, inventory.GetCount(stackableItem));
        }

        [UnityTest]
        public IEnumerator SubtractPartialStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, testCount / 2);

            yield return null;

            // Number removed.
            Assert.AreEqual(testCount - (testCount / 2), inventory.GetCount(stackableItem));
        }

        [UnityTest]
        public IEnumerator SubtractWholeStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, testCount);

            yield return null;

            // Number removed.
            Assert.AreEqual(0, inventory.GetCount(stackableItem));

            // Item removed from inventory.
            Assert.False(inventory.Contains(stackableItem));
        }

        [UnityTest]
        public IEnumerator SubtractWholeStackOverkillTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, 2 * testCount);

            yield return null;

            // Number removed.
            Assert.AreEqual(0, inventory.GetCount(stackableItem));

            // Item removed from inventory.
            Assert.False(inventory.Contains(stackableItem));
        }

        [UnityTest]
        public IEnumerator ContainsTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item otherItem = new Item();

            inventory.Add(item, 1);

            yield return null;

            Assert.True(inventory.Contains(item));
            Assert.False(inventory.Contains(otherItem));
        }

        [UnityTest]
        public IEnumerator ContainsAmountTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);

            yield return null;

            // Test under amount
            Assert.False(inventory.Contains(stackableItem, testCount + 1));

            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);

            // Test over amount.
            Assert.True(inventory.Contains(stackableItem, testCount + 1), "Incorrect over amount.");

            // test equal amount.
            Assert.True(inventory.Contains(stackableItem, 3 * testCount), "Incorrect equal amount.");
        }

        [UnityTest]
        public IEnumerator GetCountTest()
        {
            int testCount = 3;
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item otherItem = new Item();
            item.IsStackable = true;
            otherItem.IsStackable = true;

            inventory.Add(item, testCount);
            inventory.Add(otherItem, 2 * testCount);

            yield return null;

            Assert.AreEqual(testCount, inventory.GetCount(item));
            Assert.AreEqual(2 * testCount, inventory.GetCount(otherItem));
        }

        [UnityTest]
        public IEnumerator RemoveItemTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item firstItem = new Item();
            Item otherItem = new Item();
            Item lastItem = new Item();

            inventory.Add(firstItem, 4);
            inventory.Add(otherItem, 70);
            inventory.Add(lastItem, 5);

            inventory.Remove(firstItem);
            inventory.Remove(otherItem);
            inventory.Remove(lastItem);

            yield return null;

            // Removing items slots tests
            Assert.False(inventory.Contains(firstItem));
            Assert.False(inventory.Contains(otherItem));
            Assert.False(inventory.Contains(lastItem));
        }
    }
}