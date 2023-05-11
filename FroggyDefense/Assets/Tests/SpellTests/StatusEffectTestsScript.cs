using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Tests
{
    public class StatusEffectTestsScript
    {
        [UnityTest]
        public IEnumerator ApplySlowTest()
        {
            // Check if the movement speed is slowed.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator ApplyMultipleSlowsTest()
        {
            // Check that only the strongest slow is taken.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator ApplyStunTest()
        {
            // Check that movement speed is completely stopped.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator ApplyMultipleStunsTest()
        {
            // Check that all stuns are counting down.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator ApplyStunAndSlowTest()
        {
            // Check that the slow is still counting down while stunned.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator StatusEffectRefreshTest()
        {
            // Check that applying an effect to a target that already has it refreshes the effect.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator StackingStatusEffectTest()
        {
            // Check that effect stacks correctly.
            yield return null;
            Assert.Fail();
        }
    }
}