using NUnit.Framework;
using UnityEngine;

namespace FroggyDefense.Core.Items.Tests
{
    public class InventoryEditorTests
    {
        private static Vector2Int RANDOM_SIZE_RANGE = new Vector2Int(20, 100);

        /// <summary>
        /// Creates a new inventory.
        /// </summary>
        /// <returns></returns>
        private static FixedInventory CreateInventory()
        {
            int num = Random.Range(RANDOM_SIZE_RANGE.x, RANDOM_SIZE_RANGE.y);
            return new FixedInventory(num);
        }

        [Test]
        public void InitInventoryTest()
        {
            int num = Random.Range(RANDOM_SIZE_RANGE.x, RANDOM_SIZE_RANGE.y);
            FixedInventory inventory = new FixedInventory(num);

            Assert.NotNull(inventory);
            Assert.AreEqual(num, inventory.Size);
            for (int i = 0; i < inventory.Size; i++)
            {
                var slot = inventory.Get(i);
                Assert.True(slot.IsEmpty);
                Assert.AreEqual(i, slot.InventoryIndex);
            }
        }

        [Test]
        public void GetCountTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;
            Item redHerring = Item.CreateTestItem();
            redHerring.IsStackable = false;

            int randomNumber = Random.Range(1, item.StackSize - 1);

            inventory.Add(item, item.StackSize);
            inventory.Add(item, item.StackSize);
            inventory.Add(redHerring, 2);
            inventory.Add(item, item.StackSize);
            inventory.Add(item, randomNumber);

            Assert.True(inventory.Contains(item.Id));
            Assert.True(inventory.Contains(redHerring.Id));
            Assert.AreEqual(3 * item.StackSize + randomNumber, inventory.GetCount(item.Id), "Incorrect number of items.");
            Assert.AreEqual(2, inventory.GetCount(redHerring.Id), "Incorrect number of herring.");
        }

        [Test]
        public void GetStacksTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;
            Item redHerring = Item.CreateTestItem();
            redHerring.IsStackable = false;

            int randomNumber = Random.Range(1, item.StackSize - 1);

            inventory.Add(item, item.StackSize);
            inventory.Add(item, item.StackSize);
            inventory.Add(redHerring, 2);
            inventory.Add(item, item.StackSize);
            inventory.Add(item, randomNumber);

            Assert.True(inventory.Contains(item.Id));
            Assert.True(inventory.Contains(redHerring.Id));
            Assert.AreEqual(4, inventory.GetStacks(item.Id), "Incorrect number of items stacks.");
            Assert.AreEqual(2, inventory.GetStacks(redHerring.Id), "Incorrect number of herring stacks.");
        }

        [Test]
        public void AddStackableTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;

            inventory.Add(item, item.StackSize / 2);
            inventory.Add(item, item.StackSize / 2);
            inventory.Add(item, item.StackSize / 2);

            Assert.True(inventory.Contains(item.Id));
            Assert.AreEqual(1.5 * item.StackSize, inventory.GetCount(item.Id), "Incorrect number of items.");
            Assert.AreEqual(2, inventory.GetStacks(item.Id), "Incorrect number of item stacks.");
        }

        [Test]
        public void AddUnstackableTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = false;

            inventory.Add(item, 1);
            inventory.Add(item, 2);
            inventory.Add(item, 3);

            Assert.True(inventory.Contains(item.Id));
            Assert.AreEqual(6, inventory.GetCount(item.Id), "Incorrect number of items.");
            Assert.AreEqual(6, inventory.GetStacks(item.Id), "Incorrect number of item stacks.");
        }

        [Test]
        public void AddMixedTest()
        {
            FixedInventory inventory = CreateInventory();
            Item unstackable = Item.CreateTestItem();
            unstackable.IsStackable = false;
            Item stackable = Item.CreateTestItem();
            stackable.IsStackable = true;

            inventory.Add(unstackable, 2);
            inventory.Add(stackable, stackable.StackSize / 2);
            inventory.Add(unstackable, 1);
            inventory.Add(stackable, stackable.StackSize / 2);
            inventory.Add(stackable, stackable.StackSize / 2);
            inventory.Add(unstackable, 3);

            Assert.True(inventory.Contains(unstackable.Id));
            Assert.AreEqual(6, inventory.GetCount(unstackable.Id), "Incorrect number of items.");
            Assert.AreEqual(6, inventory.GetStacks(unstackable.Id), "Incorrect number of item stacks.");
            Assert.True(inventory.Contains(stackable.Id));
            Assert.AreEqual(1.5 * stackable.StackSize, inventory.GetCount(stackable.Id), "Incorrect number of items.");
            Assert.AreEqual(2, inventory.GetStacks(stackable.Id), "Incorrect number of item stacks.");
        }

        [Test]
        public void SubtractStackableTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;

            inventory.Add(item, item.StackSize / 2);
            inventory.Subtract(item.Id, 3);
            inventory.Add(item, item.StackSize / 2);
            inventory.Add(item, item.StackSize / 2);
            inventory.Subtract(item.Id, 2);
            inventory.Add(item, 2);

            Assert.True(inventory.Contains(item.Id));
            Assert.AreEqual((int)(1.5 * item.StackSize) - 3, inventory.GetCount(item.Id), "Incorrect number of items.");
            Assert.AreEqual(2, inventory.GetStacks(item.Id), "Incorrect number of item stacks.");

            inventory.Subtract(item.Id, inventory.GetCount(item.Id) / 2);

            Assert.True(inventory.Contains(item.Id));

            inventory.Subtract(item.Id, inventory.GetCount(item.Id));

            Assert.False(inventory.Contains(item.Id));
        }

        [Test]
        public void SubtractUnstackableTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = false;

            inventory.Add(item, 1);
            inventory.Subtract(item.Id, 2);
            inventory.Add(item, 2);
            inventory.Add(item, 3);
            inventory.Subtract(item.Id, 2);

            Assert.True(inventory.Contains(item.Id));
            Assert.AreEqual(3, inventory.GetCount(item.Id), "Incorrect number of items.");
            Assert.AreEqual(3, inventory.GetStacks(item.Id), "Incorrect number of item stacks.");

            inventory.Subtract(item.Id, inventory.GetCount(item.Id) / 2);

            Assert.True(inventory.Contains(item.Id));

            inventory.Subtract(item.Id, inventory.GetCount(item.Id));

            Assert.False(inventory.Contains(item.Id));
        }

        //[Test]
        //public void AddToExistingStackTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item item = new Item();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 99;

        //    inventory.Add(item, testCount);
        //    inventory.Add(stackableItem, testCount);
        //    inventory.Add(stackableItem, testCount);
        //    inventory.Add(stackableItem, testCount);

        //    // Unstackable items always have a value of 1.
        //    Assert.AreEqual(1, inventory.GetCount(item.Id));

        //    // Stackable items can have an amount.
        //    Assert.AreEqual(3 * testCount, inventory.GetCount(stackableItem.Id));

        //    // Should only be two stacks.
        //    Assert.AreEqual(1 + ((3 * testCount) / stackableItem.StackSize) + ((3 * testCount) % stackableItem.StackSize > 0 ? 1 : 0), inventory.Size);
        //}

        //[Test]
        //public void SubtractNegativeStackTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 300;

        //    inventory.Add(stackableItem, testCount);
        //    inventory.Subtract(stackableItem, -1);

        //    // Nothing removed.
        //    Assert.AreEqual(testCount, inventory.GetCount(stackableItem.Id));
        //}

        //[Test]
        //public void SubtractPartialStackTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 300;

        //    inventory.Add(stackableItem, testCount);
        //    inventory.Subtract(stackableItem, testCount / 2);

        //    // Number removed.
        //    Assert.AreEqual(testCount - (testCount / 2), inventory.GetCount(stackableItem.Id));
        //}

        //[Test]
        //public void SubtractWholeStackTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 300;

        //    inventory.Add(stackableItem, testCount);
        //    inventory.Subtract(stackableItem, testCount);

        //    // Number removed.
        //    Assert.AreEqual(0, inventory.GetCount(stackableItem.Id));

        //    // Item removed from inventory.
        //    Assert.False(inventory.Contains(stackableItem));
        //}

        //[Test]
        //public void SubtractWholeStackOverkillTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 300;

        //    inventory.Add(stackableItem, testCount);
        //    inventory.Subtract(stackableItem, 2 * testCount);

        //    // Number removed.
        //    Assert.AreEqual(0, inventory.GetCount(stackableItem.Id));

        //    // Number removed.
        //    Assert.AreEqual(0, inventory.Size);

        //    // Item removed from inventory.
        //    Assert.False(inventory.Contains(stackableItem));
        //}

        //[Test]
        //public void ContainsTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item item = new Item();
        //    Item otherItem = new Item();

        //    inventory.Add(item, 1);

        //    Assert.True(inventory.Contains(item));
        //    Assert.False(inventory.Contains(otherItem));
        //}

        //[Test]
        //public void ContainsAmountTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item stackableItem = new Item();
        //    stackableItem.IsStackable = true;
        //    int testCount = 300;

        //    inventory.Add(stackableItem, testCount);

        //    // Test under amount
        //    Assert.False(inventory.Contains(stackableItem, testCount + 1));

        //    inventory.Add(stackableItem, testCount);
        //    inventory.Add(stackableItem, testCount);

        //    // Test over amount.
        //    Assert.True(inventory.Contains(stackableItem, testCount + 1), "Incorrect over amount.");

        //    // test equal amount.
        //    Assert.True(inventory.Contains(stackableItem, 3 * testCount), "Incorrect equal amount.");
        //}

        //[Test]
        //public void GetCountTest()
        //{
        //    int testCount = 3;
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item item = new Item();
        //    Item otherItem = new Item();
        //    item.IsStackable = true;
        //    otherItem.IsStackable = true;
        //    item.Name = "A";
        //    item.Name = "B";

        //    inventory.Add(item, testCount);
        //    inventory.Add(otherItem, 2 * testCount);

        //    Assert.AreEqual(testCount, inventory.GetCount(item.Id));
        //    Assert.AreEqual(2 * testCount, inventory.GetCount(otherItem.Id));
        //}

        //[Test]
        //public void RemoveItemTest()
        //{
        //    GameObject gameObject = new GameObject();
        //    Inventory inventory = gameObject.AddComponent<Inventory>();
        //    inventory.InitInventory();
        //    Item firstItem = new Item();
        //    Item otherItem = new Item();
        //    Item lastItem = new Item();

        //    inventory.Add(firstItem, 4);
        //    inventory.Add(otherItem, 70);
        //    inventory.Add(lastItem, 5);

        //    inventory.Remove(firstItem);
        //    inventory.Remove(otherItem);
        //    inventory.Remove(lastItem);

        //    // Removing items slots tests
        //    Assert.False(inventory.Contains(firstItem));
        //    Assert.False(inventory.Contains(otherItem));
        //    Assert.False(inventory.Contains(lastItem));
        //}
    }
}