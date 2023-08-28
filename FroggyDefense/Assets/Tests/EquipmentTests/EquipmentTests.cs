using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Tests
{
    public class EquipmentTests
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
            Assert.AreEqual(hat, character.GetEquipment((int)EquipmentSlot.Hat), "Incorrect Hat.");
            Assert.AreEqual(clothes, character.GetEquipment((int)EquipmentSlot.Clothes), "Incorrect Clothes.");
            Assert.AreEqual(boots, character.GetEquipment((int)EquipmentSlot.Boots), "Incorrect Boots.");
            Assert.AreEqual(ring, character.GetEquipment((int)EquipmentSlot.Ring), "Incorrect Ring.");
            Assert.AreEqual(trinket, character.GetEquipment((int)EquipmentSlot.Trinket), "Incorrect Trinket.");
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
            Assert.AreEqual(newHat, character.GetEquipment((int)EquipmentSlot.Hat), "Incorrect Hat.");
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
            hat.AddStat(StatType.Strength, 4);
            hat.AddStat(StatType.Intellect, 205);

            // Store current stats.
            StatSheet stats = character.GetStats();
            float strength = stats.GetTotalStat(StatType.Strength);
            float Endurance = stats.GetTotalStat(StatType.Endurance);
            float agility = stats.GetTotalStat(StatType.Agility);
            float intellect = stats.GetTotalStat(StatType.Intellect);
            float spirit = stats.GetTotalStat(StatType.Spirit);

            // Equip item
            character.Equip(hat);
            yield return null;

            // Check if stats updated correctly.
            Assert.AreEqual(strength + hat.GetStat(StatType.Strength), stats.GetTotalStat(StatType.Strength), "Incorrect Strength.");
            Assert.AreEqual(Endurance + hat.GetStat(StatType.Endurance), stats.GetTotalStat(StatType.Endurance), "Incorrect Endurance.");
            Assert.AreEqual(agility + hat.GetStat(StatType.Agility), stats.GetTotalStat(StatType.Agility), "Incorrect Agility.");
            Assert.AreEqual(intellect + hat.GetStat(StatType.Intellect), stats.GetTotalStat(StatType.Intellect), "Incorrect Intellect.");
            Assert.AreEqual(spirit + hat.GetStat(StatType.Spirit), stats.GetTotalStat(StatType.Spirit), "Incorrect Spirit.");
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
            hat.AddStat(StatType.Strength, 4);
            hat.AddStat(StatType.Intellect, 205);

            // Equip item
            character.Equip(hat);
            yield return null;

            // Store current stats.
            StatSheet stats = character.GetStats();
            float strength = stats.GetTotalStat(StatType.Strength);
            float Endurance = stats.GetTotalStat(StatType.Endurance);
            float agility = stats.GetTotalStat(StatType.Agility);
            float intellect = stats.GetTotalStat(StatType.Intellect);
            float spirit = stats.GetTotalStat(StatType.Spirit);

            // Unequip item
            character.Unequip((int)EquipmentSlot.Hat);
            yield return null;

            // Check if stats updated correctly.
            Assert.AreEqual(strength - hat.GetStat(StatType.Strength), stats.GetTotalStat(StatType.Strength), "Incorrect Strength.");
            Assert.AreEqual(Endurance - hat.GetStat(StatType.Endurance), stats.GetTotalStat(StatType.Endurance), "Incorrect Endurance.");
            Assert.AreEqual(agility - hat.GetStat(StatType.Agility), stats.GetTotalStat(StatType.Agility), "Incorrect Agility.");
            Assert.AreEqual(intellect - hat.GetStat(StatType.Intellect), stats.GetTotalStat(StatType.Intellect), "Incorrect Intellect.");
            Assert.AreEqual(spirit - hat.GetStat(StatType.Spirit), stats.GetTotalStat(StatType.Spirit), "Incorrect Spirit.");
        }
    }
}