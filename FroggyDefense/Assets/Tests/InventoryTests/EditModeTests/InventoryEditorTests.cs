using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace FroggyDefense.Core.Items.Tests
{
    public class InventoryEditorTests
    {
        /*
         * TODO: Things that take specific Items should check for that specific instance/ equivalent to it.
         * 
         */


        [Test]
        public void AddSingleTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();

            inventory.Add(item, 1);

            Assert.True(inventory.Contains(item.Id));
        }

        [Test]
        public void AddAmountTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 301;

            inventory.Add(item, testCount);
            inventory.Add(stackableItem, testCount);

            // Unstackable items always have a value of 1.
            Assert.AreEqual(1, inventory.GetCount(item.Id), "Incorrect numbetr of unstackable items.");

            // Stackable items can have an amount.
            Assert.AreEqual(testCount, inventory.GetCount(stackableItem.Id), "Incorrect number of stackable items.");

            // 1 stack for unstackable item, 3 for the stackable item.
            int num = (testCount / stackableItem.StackSize);
            Assert.AreEqual(1 + num + (testCount % stackableItem.StackSize > 0 ? 1 : 0), inventory.Size, "Incorrect total number of stacks");
        }

        [Test]
        public void AddToExistingStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 99;

            inventory.Add(item, testCount);
            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);

            // Unstackable items always have a value of 1.
            Assert.AreEqual(1, inventory.GetCount(item.Id));

            // Stackable items can have an amount.
            Assert.AreEqual(3 * testCount, inventory.GetCount(stackableItem.Id));

            // Should only be two stacks.
            Assert.AreEqual(1 + ((3 * testCount) / stackableItem.StackSize) + ((3 * testCount) % stackableItem.StackSize > 0 ? 1 : 0), inventory.Size);
        }

        [Test]
        public void SubtractNegativeStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, -1);

            // Nothing removed.
            Assert.AreEqual(testCount, inventory.GetCount(stackableItem.Id));
        }

        [Test]
        public void SubtractPartialStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, testCount / 2);

            // Number removed.
            Assert.AreEqual(testCount - (testCount / 2), inventory.GetCount(stackableItem.Id));
        }

        [Test]
        public void SubtractWholeStackTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, testCount);

            // Number removed.
            Assert.AreEqual(0, inventory.GetCount(stackableItem.Id));

            // Item removed from inventory.
            Assert.False(inventory.Contains(stackableItem));
        }

        [Test]
        public void SubtractWholeStackOverkillTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);
            inventory.Subtract(stackableItem, 2 * testCount);

            // Number removed.
            Assert.AreEqual(0, inventory.GetCount(stackableItem.Id));

            // Number removed.
            Assert.AreEqual(0, inventory.Size);

            // Item removed from inventory.
            Assert.False(inventory.Contains(stackableItem));
        }

        [Test]
        public void ContainsTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item otherItem = new Item();

            inventory.Add(item, 1);

            Assert.True(inventory.Contains(item));
            Assert.False(inventory.Contains(otherItem));
        }

        [Test]
        public void ContainsAmountTest()
        {
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item stackableItem = new Item();
            stackableItem.IsStackable = true;
            int testCount = 300;

            inventory.Add(stackableItem, testCount);

            // Test under amount
            Assert.False(inventory.Contains(stackableItem, testCount + 1));

            inventory.Add(stackableItem, testCount);
            inventory.Add(stackableItem, testCount);

            // Test over amount.
            Assert.True(inventory.Contains(stackableItem, testCount + 1), "Incorrect over amount.");

            // test equal amount.
            Assert.True(inventory.Contains(stackableItem, 3 * testCount), "Incorrect equal amount.");
        }

        [Test]
        public void GetCountTest()
        {
            int testCount = 3;
            GameObject gameObject = new GameObject();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.InitInventory();
            Item item = new Item();
            Item otherItem = new Item();
            item.IsStackable = true;
            otherItem.IsStackable = true;
            item.Name = "A";
            item.Name = "B";

            inventory.Add(item, testCount);
            inventory.Add(otherItem, 2 * testCount);

            Assert.AreEqual(testCount, inventory.GetCount(item.Id));
            Assert.AreEqual(2 * testCount, inventory.GetCount(otherItem.Id));
        }

        [Test]
        public void RemoveItemTest()
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

            // Removing items slots tests
            Assert.False(inventory.Contains(firstItem));
            Assert.False(inventory.Contains(otherItem));
            Assert.False(inventory.Contains(lastItem));
        }
    }
}