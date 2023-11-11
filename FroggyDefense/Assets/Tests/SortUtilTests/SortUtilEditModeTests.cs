using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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

        SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList,
            testRejects
            );

        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(animal.Speed <= 5);
        }
    }

    [Test]
    public void AcceptedTest()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList,
            testRejects
            );

        foreach (Animal animal in testList)
        {
            Assert.IsTrue(animal.Speed > 5);
        }
    }

    [Test]
    public void FilterFuncTest1()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Size > 5; },
            testList,
            testRejects
            );

        foreach (Animal animal in testList)
        {
            Assert.IsTrue(animal.Speed > 5);
        }
        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(animal.Speed <= 5);
        }
    }

    [Test]
    public void FilterFuncTest2()
    {
        List<Animal> testList = CreateTestList();
        List<Animal> testRejects = new List<Animal>();

        SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Name.Contains('a'); },
            testList,
            testRejects
            );

        foreach (Animal animal in testList)
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

        SortUtil<Animal>.FilterList(
            (Animal animal) => { return animal.Speed > 6; },
            testList,
            testRejects
            );

        foreach (Animal animal in testList)
        {
            Assert.IsTrue(animal.Speed > 6);
        }
        foreach (Animal animal in testRejects)
        {
            Assert.IsTrue(animal.Speed <= 6);
        }
    }
}
