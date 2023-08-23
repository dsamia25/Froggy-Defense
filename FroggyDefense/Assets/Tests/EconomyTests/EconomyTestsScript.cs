using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using FroggyDefense.Economy;
using FroggyDefense.Economy.Tests;
using FroggyDefense.Core;

namespace FroggyDefense.Tests
{
    public class EconomyTestsScript
    {
        private static string TEST_SCENE = "GroundItemTestScene";
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
        public IEnumerator AddRealCurrencyTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            yield return null;

            Assert.AreEqual(1, wallet.Add(manager.testCurrency, 1));

            wallet.Add(manager.testCurrency, 1);
            wallet.Add(manager.testCurrency, 5);
            wallet.Add(manager.testCurrency, 7);

            Assert.AreEqual(14, wallet.GetAmount(manager.testCurrency));
        }

        [UnityTest]
        public IEnumerator AddUnknownFakeCurrencyTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();
            CurrencyObject unknownCurrency = ScriptableObject.CreateInstance<CurrencyObject>();

            wallet.Add(unknownCurrency, 1);

            Assert.AreEqual(0, wallet.Add(unknownCurrency, 1));
            Assert.AreEqual(0, wallet.GetAmount(unknownCurrency));
        }

        [UnityTest]
        public IEnumerator ChargeTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            Assert.AreEqual(100, wallet.Add(manager.testCurrency, 100));
            Assert.AreEqual(100, wallet.GetAmount(manager.testCurrency));
            Assert.AreEqual(50, wallet.Charge(manager.testCurrency, 50));
            Assert.AreEqual(50, wallet.GetAmount(manager.testCurrency));
            Assert.AreEqual(50, wallet.Charge(manager.testCurrency, 50));
            Assert.AreEqual(0, wallet.GetAmount(manager.testCurrency));
        }

        [UnityTest]
        public IEnumerator OverchargeTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            Assert.AreEqual(100, wallet.Add(manager.testCurrency, 100));
            Assert.AreEqual(100, wallet.GetAmount(manager.testCurrency));
            Assert.AreEqual(0, wallet.Charge(manager.testCurrency, 101));
            Assert.AreEqual(100, wallet.GetAmount(manager.testCurrency));
        }

        [UnityTest]
        public IEnumerator StartingAmountTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            Assert.AreEqual(1, wallet.Add(manager.testStartingAmtPosCurrency, 1));
            Assert.AreEqual(manager.testStartingAmtPosCurrency.StartingAmount + 1, wallet.GetAmount(manager.testStartingAmtPosCurrency));
            Assert.AreEqual(2, wallet.Add(manager.testStartingAmtNegCurrency, 2));
            Assert.AreEqual(manager.testStartingAmtNegCurrency.StartingAmount + 2, wallet.GetAmount(manager.testStartingAmtNegCurrency));
        }

        [UnityTest]
        public IEnumerator MinimumAmountTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            Assert.AreEqual(100, wallet.Add(manager.testMinAmtCurrency, 100));
            Assert.AreEqual(100, wallet.GetAmount(manager.testMinAmtCurrency));
            Assert.AreEqual(0, wallet.Charge(manager.testMinAmtCurrency, wallet.GetAmount(manager.testMinAmtCurrency) + Mathf.Abs(manager.testMinAmtCurrency.MinimumAmount) + 1));
            Assert.AreEqual(100, wallet.GetAmount(manager.testMinAmtCurrency));
        }

        [UnityTest]
        public IEnumerator MaximumAmountTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test objects.
            EconomyTestManager manager = GameObject.Find("EconomyTestManager").GetComponent<EconomyTestManager>();
            Player player = GameObject.Find("Player").GetComponent<Player>();

            // Check that load was successful.
            if (manager == null) Assert.Fail("Error: test manager not found.");
            if (player == null) Assert.Fail("Error: test player not found.");

            CurrencyWallet wallet = player.GetComponent<CurrencyWallet>();

            Assert.AreEqual(manager.testMaxAmtCurrency.MaximumAmount / 2, wallet.Add(manager.testMaxAmtCurrency, manager.testMaxAmtCurrency.MaximumAmount / 2));
            Assert.AreEqual(manager.testMaxAmtCurrency.MaximumAmount / 2, wallet.GetAmount(manager.testMaxAmtCurrency));
            Assert.AreEqual(manager.testMaxAmtCurrency.MaximumAmount / 2, wallet.Add(manager.testMaxAmtCurrency, manager.testMaxAmtCurrency.MaximumAmount / 2));
            Assert.AreEqual(manager.testMaxAmtCurrency.MaximumAmount, wallet.GetAmount(manager.testMaxAmtCurrency));
            Assert.AreEqual(0, wallet.Add(manager.testMaxAmtCurrency, manager.testMaxAmtCurrency.MaximumAmount / 2));
            Assert.AreEqual(manager.testMaxAmtCurrency.MaximumAmount, wallet.GetAmount(manager.testMaxAmtCurrency));
        }
    }
}
