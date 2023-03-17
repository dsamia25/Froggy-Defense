using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core;
using FroggyDefense.Items;

namespace FroggyDefense.Tests
{
    public class EquipmentTests : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator EquipItemTest()
        {
            // Init objects
            GameObject gameObject = new GameObject();
            Character character = gameObject.AddComponent<Character>();
            Equipment hat = new Equipment();
            Equipment clothes = new Equipment();
            Equipment boots = new Equipment();
            Equipment ring = new Equipment();
            Equipment trinket = new Equipment();

            // Set up items
            hat.Slot = EquipmentSlot.Hat;
            clothes.Slot = EquipmentSlot.Clothes;
            boots.Slot = EquipmentSlot.Boots;
            ring.Slot = EquipmentSlot.Ring;
            trinket.Slot = EquipmentSlot.Trinket;

            // Equip item
            character.Equip(hat);
            character.Equip(clothes);
            character.Equip(boots);
            character.Equip(ring);
            character.Equip(trinket);
            yield return null;

            // Check if equipped.
            Assert.AreEqual(hat, character.HatSlot, "Incorrect Hat.");
            Assert.AreEqual(clothes, character.ClothesSlot, "Incorrect Clothes.");
            Assert.AreEqual(boots, character.BootsSlot, "Incorrect Boots.");
            Assert.AreEqual(ring, character.RingSlot, "Incorrect Ring.");
            Assert.AreEqual(trinket, character.TrinketSlot, "Incorrect Trinket.");
        }

        [UnityTest]
        public IEnumerator EquipNewItemTest()
        {
            // Init objects
            GameObject gameObject = new GameObject();
            Character character = gameObject.AddComponent<Character>();
            Equipment hat = new Equipment();
            Equipment newHat = new Equipment();

            // Set up items
            hat.Name = "Cool Hat";
            hat.Slot = EquipmentSlot.Hat;

            newHat.Name = "Even Cooler Hat";
            newHat.Slot = EquipmentSlot.Hat;
            yield return null;

            // Equip item
            character.Equip(hat);

            // Equip replacement item
            character.Equip(newHat);

            // Check if equipped.
            Assert.AreEqual(newHat, character.HatSlot, "Incorrect Hat.");
        }

        [UnityTest]
        public IEnumerator UpdateStatsIncreaseTest()
        {
            // Init objects
            GameObject gameObject = new GameObject();
            Character character = gameObject.AddComponent<Character>();
            Equipment hat = new Equipment();

            // Set up item
            hat.Name = "Cool Hat";
            hat.Slot = EquipmentSlot.Hat;
            hat.Strength = 4;
            hat.Intellect = 205;

            // Store current stats.
            float strength = character.Strength;
            float dexterity = character.Dexterity;
            float agility = character.Agility;
            float intellect = character.Intellect;
            float spirit = character.Spirit;

            // Equip item
            character.Equip(hat);
            yield return null;

            // Check if stats updated correctly.
            Assert.AreEqual(strength + hat.Strength, character.Strength, "Incorrect Strength.");
            Assert.AreEqual(dexterity + hat.Dexterity, character.Dexterity, "Incorrect Dexterity.");
            Assert.AreEqual(agility + hat.Agility, character.Agility, "Incorrect Agility.");
            Assert.AreEqual(intellect + hat.Intellect, character.Intellect, "Incorrect Intellect.");
            Assert.AreEqual(spirit + hat.Spirit, character.Spirit, "Incorrect Spirit.");
        }

        [UnityTest]
        public IEnumerator UpdateStatsDecreaseTest()
        {
            // Init objects
            GameObject gameObject = new GameObject();
            Character character = gameObject.AddComponent<Character>();
            Equipment hat = new Equipment();

            // Set up item
            hat.Name = "Cool Hat";
            hat.Slot = EquipmentSlot.Hat;
            hat.Strength = 4;
            hat.Intellect = 205;

            // Equip item
            character.Equip(hat);
            yield return null;

            // Store current stats.
            float strength = character.Strength;
            float dexterity = character.Dexterity;
            float agility = character.Agility;
            float intellect = character.Intellect;
            float spirit = character.Spirit;

            // Unequip item
            character.Unequip(EquipmentSlot.Hat);
            yield return null;

            // Check if stats updated correctly.
            Assert.AreEqual(strength - hat.Strength, character.Strength, "Incorrect Strength.");
            Assert.AreEqual(dexterity - hat.Dexterity, character.Dexterity, "Incorrect Dexterity.");
            Assert.AreEqual(agility - hat.Agility, character.Agility, "Incorrect Agility.");
            Assert.AreEqual(intellect - hat.Intellect, character.Intellect, "Incorrect Intellect.");
            Assert.AreEqual(spirit - hat.Spirit, character.Spirit, "Incorrect Spirit.");
        }
    }
}