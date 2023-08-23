using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestParserTestsScript
{
    private static string TEST_STRING = "In a hole in the ground there lived a hobbit. Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: it was a hobbit­hole, and that means comfort.";

    [Test]
    public void SplitBySpacesTest()
    {
        string testString = "   In a hole    in the  ground there lived a      hobbit. Not a      nasty, dirty,  wet  hole, filled with the ends of worms and an oozy smell, nor yet a dry, bare,   sandy  hole with nothing    in it to sit  down on or    to eat:    it was a hobbit­hole,   and that means comfort.   ";

        string[] splitText = TextParser.Split(testString);

        Assert.AreEqual(52, splitText.Length);
    }

    [Test]
    public void CleansTextTest()
    {
        string testString = "     In a hole 'in' the ground there lived a hobbit. \"Not    a nasty, dirty, wet hole,   filled with the ends of \"worms\"''' and an oozy smell\',  nor yet a dry,   bare,                     sandy \'hole with nothing in it \'\"\"\'to \'\'\'sit\'\'\'  down on or to eat: \"\"\"it was a hobbit­hole, and that means comfort.     ";

        testString = TextParser.Clean(testString);

        Assert.AreEqual(TEST_STRING.ToUpper(), testString.ToUpper());
    }

    [Test]
    public void FindsCommandTest()
    {
        string testInput = "SetTime ghjksh ahdiu lkji 3 08 hsakjhf";

        var output = TextParser.ParseCommand(testInput);

        Assert.AreEqual("SETTIME", output.command.ToUpper());
    }

    [Test]
    public void FindsArgsTest()
    {
        string testInput = "SetTime first second third fourth ...";

        var output = TextParser.ParseCommand(testInput);

        Assert.AreEqual(output.args.Length, 5);
        Assert.AreEqual("FIRST", output.args[0]);
        Assert.AreEqual("SECOND", output.args[1]);
        Assert.AreEqual("THIRD", output.args[2]);
        Assert.AreEqual("FOURTH", output.args[3]);
        Assert.AreEqual("...", output.args[4]);
    }

    [Test]
    public void EmptyInputTest()
    {
        string testInput = "";

        var output = TextParser.ParseCommand(testInput);

        Assert.AreEqual("", output.command.ToUpper());
        Assert.AreEqual(output.args.Length, 0);
    }

    [Test]
    public void NoArgsTest()
    {
        string testInput = "SetTime ";

        var output = TextParser.ParseCommand(testInput);

        Assert.AreEqual("SETTIME", output.command.ToUpper());
        Assert.AreEqual(output.args.Length, 0);
    }
}
