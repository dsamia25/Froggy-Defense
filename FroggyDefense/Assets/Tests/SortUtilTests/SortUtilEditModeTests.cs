using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SortUtilEditModeTests
{
    private class Animal
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }

        public Animal (string name, int size, int speed)
        {
            Name = name;
            Size = size;
            Speed = speed;
        }
    }

    private List<Animal> CreateTestList()
    {
        List<Animal> list = new List<Animal>();
        list.Add(new Animal("Dog", 4, 5));
        list.Add(new Animal("Cat", 2, 7));
        list.Add(new Animal("Bird", 1, 8));
        list.Add(new Animal("Elephant", 20, 1));
        list.Add(new Animal("Whale", 40, 3));
        list.Add(new Animal("Tiger", 10, 8));
        list.Add(new Animal("Cheetah", 7, 1000));
        list.Add(new Animal("Falcon", 10, 2000));
        list.Add(new Animal("Wolf", 5, 6));
        list.Add(new Animal("Bear", 13, 7));
        list.Add(new Animal("Gorilla", 13, 4));
        list.Add(new Animal("Monkey", 4, 5));
        list.Add(new Animal("Bigger Bear", 14, 7));
        return list;
    }

    [Test]
    public void RejectsTest()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList,
            testRejects
            );

        string str = "Rejects list:";
        foreach (Animal animal in testRejects)
        {
            str += $"\n{animal.Name}'s size is {animal.Size}.";
            Assert.IsTrue(animal.Size <= 5);
        }
        str += "}";
        Debug.Log(str);
    }

    [Test]
    public void AcceptedTest()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList,
            testRejects
            );

        string str = "Filtered list:";
        foreach (Animal animal in filteredList)
        {
            str += $"\n{animal.Name}'s size is {animal.Size}.";
            Assert.IsTrue(animal.Size > 5);
        }
        str += "}";
        Debug.Log(str);
    }

    [Test]
    public void FilterFuncTest1()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Speed > 5; },
            testList,
            testRejects
            );

        foreach (Animal animal in filteredList)
        {
            Assert.IsTrue(animal.Speed > 5);
        }
        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(animal.Speed <= 5, $"{animal.Name}'s speed is {animal.Speed}.");
        }
    }

    [Test]
    public void FilterFuncTest2()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Name.Contains('a'); },
            testList,
            testRejects
            );

        foreach (Animal animal in filteredList)
        {
            Assert.IsTrue(animal.Name.Contains('a'));
        }
        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(!animal.Name.Contains('a'));
        }
    }

    [Test]
    public void FilterFuncTest3()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Speed > 6; },
            testList,
            testRejects
            );

        foreach (Animal animal in filteredList)
        {
            Assert.IsTrue(animal.Speed > 6);
        }
        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(animal.Speed <= 6);
        }
    }

    [Test]
    public void NoRejectsListTest()
    {
        List<Animal> testList = CreateTestList();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Speed > 6; },
            testList
            );

        foreach (Animal animal in filteredList)
        {
            Assert.IsTrue(animal.Speed > 6);
        }
    }

    [Test]
    public void NullFuncListTest()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = null;

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            null,
            testList,
            testRejects
            );

        for (int i = 0; i < filteredList.Count; i++)
        {
            Assert.AreEqual(filteredList[i], testList[i]);
        }

        filteredList = SortUtil<Animal>.FilterList(
            null,
            testList
            );

        for (int i = 0; i < filteredList.Count; i++)
        {
            Assert.AreEqual(filteredList[i], testList[i]);
        }
    }

    [Test]
    public void NullRejectsListTest()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Speed > 6; },
            testList,
            testRejects
            );

        foreach (Animal animal in filteredList)
        {
            Assert.IsTrue(animal.Speed > 6);
        }
    }

    [Test]
    public void NullInputListTest()
    {
        List<Animal> testList = null;
        List<Animal> testRejects = new List<Animal>();

        Assert.Throws<ArgumentNullException>(
            delegate
            {
                SortUtil<Animal>.FilterList(
                    (Animal animal) => { return animal.Speed > 6; },
                    testList
                );
            },
            $"Should be throwing a null arg exception."
        );

        Assert.Throws<ArgumentNullException>(
            delegate
            {
                SortUtil<Animal>.FilterList(
                    (Animal animal) => { return animal.Speed > 6; },
                    testList,
                    testRejects
                );
            },
            $"Should be throwing a null arg exception."
        );
    }

    [Test]
    public void SortByTest()
    {
        List<Animal> testList = CreateTestList();

        List<Animal> filteredList = SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList
            );

        filteredList.Sort((Animal a, Animal b) => { if (a.Size > b.Size) { return 1; } else if (a.Size < b.Size) { return -1; } else { return 0; } });

        for (int i = 0; i < filteredList.Count - 1; i++)
        {
            Assert.IsTrue(filteredList[i].Size <= filteredList[i + 1].Size, $"[{i}] = {filteredList[i].Size}. [{i + 1}] = {filteredList[i + 1].Size}.");
        }
    }
}
