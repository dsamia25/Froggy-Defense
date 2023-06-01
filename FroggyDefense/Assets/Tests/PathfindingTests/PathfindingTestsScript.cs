using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FroggyDefense.Core;

namespace Pathfinder.Tests
{
    public class PathfindingTestsScript
    {
        [UnityTest]
        public IEnumerator NoPathTest()
        {
            // TODO: Finds no valid path, (returns empty set).
            Assert.Fail();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SimpleLineTest()
        {
            // TODO: Makes a simple straight line.
            Assert.Fail();
            yield return null;
        }

        [UnityTest]
        public IEnumerator DiagonalLineTest()
        {
            // TODO: Makes a simple diagonal line.
            Assert.Fail();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SimpleBlockTest()
        {
            // TODO: Finds path around block in center of map.
            /*  00000
             *  XXXX0
             *  0XXX0
             *  0XXX0
             *  00000
             */
            Assert.Fail();
            yield return null;
        }

        [UnityTest]
        public IEnumerator TwoOptionsTest()
        {
            // TODO: Finds path around block in center of map, either of two options. Right then down or down then right.
            /*  00000
             *  0XXX0
             *  0XXX0
             *  0XXX0
             *  00000
             */
            Assert.Fail();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SimpleMazeTest()
        {
            // TODO: Finds path through simple maze.
            /*  00000
             *  0XXX0
             *  0XXX0
             *  0XXX0
             *  00000
             */
            Assert.Fail();
            yield return null;
        }
    }
}
