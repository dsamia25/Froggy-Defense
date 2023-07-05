using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using FroggyDefense.Interactables;

namespace FroggyDefense.Core.Items.Tests
{
    public class InventoryPlayModeTests
    {
        private static string TEST_SCENE = "GroundItemTestScene";
        private static Vector2 ITEM_SPAWN_LOC = new Vector2(100, 100);
        private bool SceneLoaded = false;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Subscribe events.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Unsubscribe events.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Marks that the test can continue now that the scene is loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator PickupItemTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            InventoryTestManager manager = GameObject.Find("InventoryTestManager").GetComponent<InventoryTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();
            Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
            IInventory inventory = player.CharacterInventory;

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");
            if (playerRb == null) Assert.Fail("Error: test player rb not found.");

            Assert.AreEqual(0, inventory.Size, "Inventory did not start at 0.");

            GameObject testItem = manager.SpawnTestItem(ITEM_SPAWN_LOC, false);
            yield return new WaitForSeconds(.05f);

            // Move the player nearby and then walk over to pick up the items.
            player.transform.position = new Vector2(ITEM_SPAWN_LOC.x - 1, ITEM_SPAWN_LOC.y - 1);
            yield return new WaitWhile(
                () => {
                    player.Move(Vector2.one);
                    return player.transform.position.x < ITEM_SPAWN_LOC.x && player.transform.position.y < ITEM_SPAWN_LOC.y;
                }
            );

            // Check test stuff.
            Assert.True(testItem == null, "testItem is not null.");
            Assert.AreEqual(1, inventory.Size, "Inventory size is incorrect.");
        }

        [UnityTest]
        public IEnumerator PickupItemMultipleNonStackingTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            InventoryTestManager manager = GameObject.Find("InventoryTestManager").GetComponent<InventoryTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();
            Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
            IInventory inventory = player.CharacterInventory;

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");
            if (playerRb == null) Assert.Fail("Error: test player rb not found.");

            Assert.AreEqual(0, inventory.Size, "Inventory did not start at 0.");

            int randomAmount = UnityEngine.Random.Range(10, 50);
            GameObject[] testItems = manager.SpawnTestItems(ITEM_SPAWN_LOC, randomAmount, false);
            yield return new WaitForSeconds(.05f);

            // Move the player nearby and then walk over to pick up the items.
            player.transform.position = new Vector2(ITEM_SPAWN_LOC.x - 1, ITEM_SPAWN_LOC.y - 1);
            yield return new WaitWhile(
                () => {
                    player.Move(Vector2.one);
                    return player.transform.position.x < ITEM_SPAWN_LOC.x && player.transform.position.y < ITEM_SPAWN_LOC.y;
                }
            );

            // Check test stuff.
            foreach (GameObject item in testItems)
            {
                Assert.True(item == null, "testItem is not null.");
            }
            Assert.AreEqual(randomAmount, inventory.Size, "Inventory size is incorrect.");
        }

        [UnityTest]
        public IEnumerator PickupItemMultipleStackingTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            InventoryTestManager manager = GameObject.Find("InventoryTestManager").GetComponent<InventoryTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();
            Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
            IInventory inventory = player.CharacterInventory;

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");
            if (playerRb == null) Assert.Fail("Error: test player rb not found.");

            Assert.AreEqual(0, inventory.Size, "Inventory did not start at 0.");

            GameObject[] testItems = manager.SpawnTestItems(ITEM_SPAWN_LOC, 3, true);
            yield return new WaitForSeconds(.05f);

            // Move the player nearby and then walk over to pick up the items.
            player.transform.position = new Vector2(ITEM_SPAWN_LOC.x - 1, ITEM_SPAWN_LOC.y - 1);
            yield return new WaitWhile(
                () => {
                    player.Move(Vector2.one);
                    return player.transform.position.x < ITEM_SPAWN_LOC.x && player.transform.position.y < ITEM_SPAWN_LOC.y;
                }
            );

            // Check test stuff.
            foreach (GameObject item in testItems)
            {
                Assert.True(item == null, "testItem is not null.");
            }
            Assert.AreEqual(1, inventory.Size, "Inventory size is incorrect.");
        }
    }
}