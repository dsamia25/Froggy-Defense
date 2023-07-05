using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Pathfinder.Tests
{
    public class PathfindingEditorTests
    {
        /*  
         *  -------------------------
         *  |   Diagram Key:        |
         *  |-----------------------|
         *  |   0 -> open space.    |
         *  |   X -> blocked tile.  |
         *  |   S -> start.         |
         *  |   F -> finish.        |
         *  |   . -> path.
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

        [Test]
        public void NoPathTest()
        {
            // Finds no valid path, (returns empty set).
            /*  
             *  00X00
             *  00X00
             *  00X00
             *  00X00
             *  S0X0F
             */
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(4, 0);
            List<Vector2Int> map = new List<Vector2Int>();

            map.Add(start);
            map.Add(finish);
            // 0,0 is start
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(0, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 0));
            map.Add(new Vector2Int(1, 1));
            map.Add(new Vector2Int(1, 2));
            map.Add(new Vector2Int(1, 3));
            map.Add(new Vector2Int(1, 4));
            // no collumn 2
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(3, 1));
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(3, 4));
            // 4,0 is finish
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(4, 3));
            map.Add(new Vector2Int(4, 4));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            Assert.NotNull(path);
            Assert.AreEqual(0, path.Count);
        }

        [Test]
        public void SimpleLineTest()
        {
            // Makes a simple straight line.
            /*  
             *  F0000
             *  .0000
             *  .0000
             *  .0000
             *  S0000
             */

            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(4, 0);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);
            // 0,0 is start
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(0, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 0));
            map.Add(new Vector2Int(1, 1));
            map.Add(new Vector2Int(1, 2));
            map.Add(new Vector2Int(1, 3));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(2, 1));
            map.Add(new Vector2Int(2, 2));
            map.Add(new Vector2Int(2, 3));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(3, 1));
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(3, 4));
            // 4,0 is finish
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(4, 3));
            map.Add(new Vector2Int(4, 4));

            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(1, 0));
            expectedPath.Add(new Vector2(2, 0));
            expectedPath.Add(new Vector2(3, 0));
            expectedPath.Add(new Vector2(4, 0));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path); Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [Test]
        public void DiagonalLineTest()
        {
            // Makes a simple diagonal line.
            /*  
             *  0000F
             *  000.0
             *  00.00
             *  0.000
             *  S0000
             */
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(4, 4);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);
            // 0,0 is start
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(0, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 0));
            map.Add(new Vector2Int(1, 1));
            map.Add(new Vector2Int(1, 2));
            map.Add(new Vector2Int(1, 3));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(2, 1));
            map.Add(new Vector2Int(2, 2));
            map.Add(new Vector2Int(2, 3));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(3, 1));
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(3, 4));
            map.Add(new Vector2Int(4, 0));
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(4, 3));
            // 4,4 is finish

            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(1, 1));
            expectedPath.Add(new Vector2(2, 2));
            expectedPath.Add(new Vector2(3, 3));
            expectedPath.Add(new Vector2(4, 4));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path);
                Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [Test]
        public void SimpleWallTest()
        {
            // Finds path around block in center of map.
            /*  
             *  0...F
             *  .X000
             *  .X000
             *  .XXX0
             *  SX000
             */
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(4, 4);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);

            // 0,0 is start
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(4, 0));
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(0, 2));
            map.Add(new Vector2Int(2, 2));
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(2, 3));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(4, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 4));
            // 4,4 is finish

            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(0, 1));
            expectedPath.Add(new Vector2(0, 2));
            expectedPath.Add(new Vector2(0, 3));
            expectedPath.Add(new Vector2(1, 4));
            expectedPath.Add(new Vector2(2, 4));
            expectedPath.Add(new Vector2(3, 4));
            expectedPath.Add(new Vector2(4, 4));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path);
                Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [Test]
        public void AroundWallTest()
        {
            // Finds path around block in center of map.
            /*  
             *  0.000
             *  .X.00
             *  .XF00
             *  .XXX0
             *  SX000
             */
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(2, 2);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);

            // 0,0 is start
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(4, 0));
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(0, 2));
            // 2,2 is finish
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(2, 3));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(4, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 4));
            map.Add(new Vector2Int(4, 4));

            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(0, 1));
            expectedPath.Add(new Vector2(0, 2));
            expectedPath.Add(new Vector2(0, 3));
            expectedPath.Add(new Vector2(1, 4));
            expectedPath.Add(new Vector2(2, 3));
            expectedPath.Add(new Vector2(2, 2));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path);
                Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [Test]
        public void TwoOptionsTest()
        {
            // Finds path around block in center of map, either of two options. Right then down or down then right.
            /*  Check for either direction.
             *  S....
             *  .XXX.
             *  .X00.
             *  .X00.
             *  ....F
             */
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int finish = new Vector2Int(2, 2);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();
            List<Vector2> alternatePath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);

            // 0,0 is start
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(4, 0));
            map.Add(new Vector2Int(0, 1));
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(0, 2));
            // 2,2 is finish
            map.Add(new Vector2Int(3, 2));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(0, 3));
            map.Add(new Vector2Int(2, 3));
            map.Add(new Vector2Int(3, 3));
            map.Add(new Vector2Int(4, 3));
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 4));
            map.Add(new Vector2Int(4, 4));

            // TODO: Make an alternate path to check.
            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(0, 1));
            expectedPath.Add(new Vector2(0, 2));
            expectedPath.Add(new Vector2(0, 3));
            expectedPath.Add(new Vector2(1, 4));
            expectedPath.Add(new Vector2(2, 3));
            expectedPath.Add(new Vector2(2, 2));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            Assert.Fail("Not Implemented Yet.");
            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    // TODO: Make a check for the main or the alternate path value.
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path);
                Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [Test]
        public void SimpleMazeTest()
        {
            // Finds path through simple maze.
            /*  
             *  00000
             *  0XXXF
             *  000X.
             *  0SXX.
             *  00..X
             */
            Vector2Int start = new Vector2Int(1, 1);
            Vector2Int finish = new Vector2Int(4, 3);
            List<Vector2Int> map = new List<Vector2Int>();
            List<Vector2> expectedPath = new List<Vector2>();

            map.Add(start);
            map.Add(finish);

            map.Add(new Vector2Int(0, 0));
            map.Add(new Vector2Int(1, 0));
            map.Add(new Vector2Int(2, 0));
            map.Add(new Vector2Int(3, 0));
            map.Add(new Vector2Int(0, 1));
            // 1,1 is start
            map.Add(new Vector2Int(4, 1));
            map.Add(new Vector2Int(0, 2));
            map.Add(new Vector2Int(1, 2));
            map.Add(new Vector2Int(2, 2));
            map.Add(new Vector2Int(4, 2));
            map.Add(new Vector2Int(0, 3));
            // 4,3 is finish
            map.Add(new Vector2Int(0, 4));
            map.Add(new Vector2Int(1, 4));
            map.Add(new Vector2Int(2, 4));
            map.Add(new Vector2Int(3, 4));
            map.Add(new Vector2Int(4, 4));

            expectedPath.Add(new Vector2(1, 1));
            expectedPath.Add(new Vector2(2, 0));
            expectedPath.Add(new Vector2(3, 0));
            expectedPath.Add(new Vector2(4, 1));
            expectedPath.Add(new Vector2(4, 2));
            expectedPath.Add(new Vector2(4, 3));

            List<Vector2> path = GridPathfinder.FindShortestPath(map, start, finish);

            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], path[i]);
                }
            }
            catch (Exception ex)
            {
                string str = PathToString(path);
                Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }
    }
}