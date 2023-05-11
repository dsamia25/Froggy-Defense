using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Tests
{
    public class SpellTestsScript
    {
        [UnityTest]
        public IEnumerator DamageActionTest()
        {
            // Check that correct amount of damage, correct damage type, and caster dealt.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator SpellEffectRadiusTest()
        {
            // Check that units in the radius take damage, outside do not.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator SpellTargetRangeTest()
        {
            // Check that trying to cast spell outside of range only casts it at furthest possible range.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator SpellAppliedEffectsTest()
        {
            // Check that the spell applies its status effect.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator SpellAoeCappedTargetTest()
        {
            // Check that a capped spell only effects the maximum target cap.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator DamageAreaTest()
        {
            // Check that a damage area is created and lasts the correct amount of time and does all damage ticks.
            yield return null;
            Assert.Fail();
        }

        [UnityTest]
        public IEnumerator DamageAreaAppliedEffectsTest()
        {
            // Check that the damage area applies its status effect.
            yield return null;
            Assert.Fail();
        }
    }
}