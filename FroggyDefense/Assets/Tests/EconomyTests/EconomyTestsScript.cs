using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Economy;

namespace FroggyDefense.Tests
{
    public class EconomyTestsScript
    {
        [UnityTest]
        public IEnumerator AddTest()
        {
            // TODO: Test if currencies are added correctly.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator AddUnknownRealCurrencyTest()
        {
            // TODO: Test if a currency not already in the list is added correctly.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator AddUnknownFakeCurrencyTest()
        {
            // TODO: Test if a currency not in the currency list is rejected.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator ChargeTest()
        {
            // TODO: Test if charging currencies correctly.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator StartingAmountTest()
        {
            // TODO: Test if the correct starting amount is added.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator MinimumAmountTest()
        {
            // TODO: Test if the minimum amount is enforced.

            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator MaximumAmountTest()
        {
            // TODO: Test if the maximum amount is enforced.

            yield return null;
            Assert.Fail();
        }
    }
}
