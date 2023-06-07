using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Pathfinder.Tests
{
    public class PathfindingTestsScript
    {
        /*  
         *  -------------------------
         *  |   Diagram Key:        |
         *  |-----------------------|
         *  |   0 -> normal space.  |
         *  |   W -> path tile.     |
         *  |   X -> blocked tile.  |
         *  |   S -> start.         |
         *  |   F -> finish.        |
         *  |   . -> path.          |
         *  -------------------------
         */

        /// <summary>
        /// Makes a string list of each point on the path with one point per line.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string PathToString(List<Vector2> path)
        {
            string str = "{\n";
            for (int i = 0; i < path.Count; i++)
            {
                str += "\t" + path[i].ToString() + (i < path.Count - 1 ? "," : "") + "\n";
            }
            str += "}";
            return str;
        }

        /// <summary>
        /// Initialize a test board with no path layers to use for the pathing tests.
        /// </summary>
        private void InitBoard()
        {

        }

        /// <summary>
        /// Initializes a board with path layers for pathing tests.
        /// </summary>
        private void InitBoardWithPaths()
        {

        }

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
