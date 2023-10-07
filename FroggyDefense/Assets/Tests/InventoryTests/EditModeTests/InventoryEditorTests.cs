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

        [Test]
        public void RemoveTest()
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

            inventory.Remove(item.Id);

            Assert.False(inventory.Contains(item.Id));
        }

        [Test]
        public void GetTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;
            Item redHerring = Item.CreateTestItem();
            redHerring.IsStackable = false;

            int randomNumber = Random.Range(1, item.StackSize - 1);

            Assert.IsNull(inventory.Get(-1));
            Assert.IsNull(inventory.Get(inventory.Size));

            Assert.AreEqual(null, inventory.Get(0).item);
            Assert.AreEqual(0, inventory.Get(0).InventoryIndex);

            inventory.Add(item, item.StackSize);
            inventory.Add(redHerring, 3);
            inventory.Add(item, item.StackSize);

            Assert.AreEqual(item, inventory.Get(0).item);
            Assert.AreEqual(0, inventory.Get(0).InventoryIndex);
            Assert.AreEqual(redHerring, inventory.Get(1).item);
            Assert.AreEqual(1, inventory.Get(1).InventoryIndex);
            Assert.AreEqual(redHerring, inventory.Get(2).item);
            Assert.AreEqual(2, inventory.Get(2).InventoryIndex);
            Assert.AreEqual(redHerring, inventory.Get(3).item);
            Assert.AreEqual(3, inventory.Get(3).InventoryIndex);
            Assert.AreEqual(item, inventory.Get(4).item);
            Assert.AreEqual(4, inventory.Get(4).InventoryIndex);
        }

        [Test]
        public void OrderTest()
        {
            FixedInventory inventory = CreateInventory();
            Item item = Item.CreateTestItem();
            item.IsStackable = true;
            Item redHerring = Item.CreateTestItem();
            redHerring.IsStackable = false;

            int randomNumber = Random.Range(1, item.StackSize - 1);

            Assert.IsNull(inventory.Get(-1));
            Assert.IsNull(inventory.Get(inventory.Size));

            Assert.AreEqual(null, inventory.Get(0).item);
            Assert.AreEqual(0, inventory.Get(0).InventoryIndex);

            inventory.Add(item, item.StackSize);
            inventory.Add(redHerring, 3);
            inventory.Add(item, item.StackSize);
            inventory.Subtract(redHerring.Id, 2);

            Assert.AreEqual(item, inventory.Get(0).item);
            Assert.AreEqual(0, inventory.Get(0).InventoryIndex);
            Assert.AreEqual(redHerring, inventory.Get(1).item);
            Assert.AreEqual(1, inventory.Get(1).InventoryIndex);
            Assert.AreEqual(null, inventory.Get(2).item);
            Assert.AreEqual(2, inventory.Get(2).InventoryIndex);
            Assert.AreEqual(null, inventory.Get(3).item);
            Assert.AreEqual(3, inventory.Get(3).InventoryIndex);
            Assert.AreEqual(item, inventory.Get(4).item);
            Assert.AreEqual(4, inventory.Get(4).InventoryIndex);
        }

        [Test]
        public void SwapTwoItemsTest()
        {
            FixedInventory inventory = CreateInventory();
            Item stackable = Item.CreateTestItem();
            stackable.IsStackable = true;
            Item unstackable = Item.CreateTestItem();
            unstackable.IsStackable = false;

            int randomNumber = Random.Range(1, stackable.StackSize - 1);

            inventory.Add(stackable, randomNumber);
            inventory.Add(unstackable, 2);

            Assert.AreEqual(stackable, inventory.Get(0).item);
            Assert.AreEqual(unstackable, inventory.Get(1).item);

            inventory.Swap(0, 1);

            Assert.AreEqual(stackable, inventory.Get(1).item);
            Assert.AreEqual(unstackable, inventory.Get(0).item);
        }

        [Test]
        public void SwapOneItemTest()
        {
            FixedInventory inventory = CreateInventory();
            Item stackable = Item.CreateTestItem();
            stackable.IsStackable = true;
            Item unstackable = Item.CreateTestItem();
            unstackable.IsStackable = false;

            int randomNumber = Random.Range(1, stackable.StackSize - 1);

            inventory.Add(stackable, randomNumber);
            inventory.Add(unstackable, 2);

            Assert.AreEqual(stackable, inventory.Get(0).item);
            Assert.AreEqual(unstackable, inventory.Get(1).item);
            Assert.AreEqual(null, inventory.Get(4).item);
            Assert.AreEqual(null, inventory.Get(5).item);

            inventory.Swap(0, 4);
            inventory.Swap(1, 5);

            Assert.AreEqual(stackable, inventory.Get(4).item);
            Assert.AreEqual(unstackable, inventory.Get(5).item);
        }
    }
}