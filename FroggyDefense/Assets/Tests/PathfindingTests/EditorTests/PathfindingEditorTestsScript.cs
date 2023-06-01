using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Pathfinder;

public class PathfindingEditorTestsScript
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

    [Test]
    public void NoPathTest()
    {
        // Finds no valid path, (returns empty set).
        /*  
         *  S0X0F
         *  00X00
         *  00X00
         *  00X00
         *  00X00
         */
        Vector2Int start = new Vector2Int(0, 0);
        Vector2Int finish = new Vector2Int(4, 0);
        SortedSet<Vector2Int> map = new SortedSet<Vector2Int>();

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
         *  S...F
         *  00000
         *  00000
         *  00000
         *  00000
         */

        Vector2Int start = new Vector2Int(0, 0);
        Vector2Int finish = new Vector2Int(4, 0);
        SortedSet<Vector2Int> map = new SortedSet<Vector2Int>();
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
        } catch
        {
            Assert.Fail("ERROR");
        }
    }

    [Test]
    public void DiagonalLineTest()
    {
        // TODO: Makes a simple diagonal line.
        /*  
         *  0000F
         *  000.0
         *  00.00
         *  0.000
         *  S0000
         */
        Assert.Fail();
    }

    [Test]
    public void SimpleBlockTest()
    {
        // TODO: Finds path around block in center of map.
        /*  
         *  S....
         *  XXXX.
         *  0X00.
         *  0X00.
         *  0000F
         */
        Assert.Fail();
    }

    [Test]
    public void AroundBlockTest()
    {
        // TODO: Finds path around block in center of map.
        /*  
         *  S....
         *  0XXX.
         *  0X0F.
         *  0X000
         *  00000
         */
        Assert.Fail();
    }

    [Test]
    public void TwoOptionsTest()
    {
        // TODO: Finds path around block in center of map, either of two options. Right then down or down then right.
        /*  Check for either direction.
         *  S....
         *  .XXX.
         *  .X00.
         *  .X00.
         *  ....F
         */
        Assert.Fail();
    }

    [Test]
    public void SimpleMazeTest()
    {
        // TODO: Finds path through simple maze.
        /*  
         *  00000
         *  0XXXF
         *  000X.
         *  0SX.0
         *  00.0X
         */
        Assert.Fail();
    }
}
