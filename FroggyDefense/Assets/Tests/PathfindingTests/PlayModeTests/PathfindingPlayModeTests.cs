using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using FroggyDefense.Core;

namespace Pathfinder.Tests
{
    public class PathfindingPlayModeTests
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

        private static string TEST_SCENE = "PathfinderTestScene";
        private bool SceneLoaded = false;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Subscribe events.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Unsubscribe events.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Marks that the test can continue now that the scene is loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded = true;
        }

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
        /// Counts all the tiles in all the maps.
        /// </summary>
        /// <param name="tilemapLayers"></param>
        /// <returns></returns>
        private int CountTiles(Tilemap[] tilemapLayers)
        {
            int total = 0;
            for (int i = 0; i < tilemapLayers.Length; i++)
            {
                Tilemap layer = tilemapLayers[i];
                layer.CompressBounds();
                for (int x = Mathf.FloorToInt(layer.localBounds.min.x); x < Mathf.FloorToInt(layer.localBounds.max.x); x++)
                {
                    for (int y = Mathf.FloorToInt(layer.localBounds.min.y); y < Mathf.FloorToInt(layer.localBounds.max.y); y++)
                    {
                        if (layer.HasTile(new Vector3Int(x, y)))
                        {
                            total++;
                        }
                    }
                }
            }
            return total;
        }

        [UnityTest]
        public IEnumerator BuildNodeMapTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;
            int totalTiles = CountTiles(boardManager.FullMap);

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            // Check test stuff.
            Assert.AreEqual(totalTiles, PathfinderMap.Count);
        }

        [UnityTest]
        public IEnumerator NullMapTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;
            int totalTiles = CountTiles(boardManager.FullMap);

            // Test Actions.
            PathfinderMap = null;

            // Check test stuff.
            Assert.Throws<System.NullReferenceException>(() => TilePathfinder.FindShortestPath(PathfinderMap, Vector2Int.zero, Vector2Int.zero, new LayerInfo(false, false)));
        }

        [UnityTest]
        public IEnumerator EmptyMapTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;
            int totalTiles = CountTiles(boardManager.FullMap);

            // Test Actions.
            PathfinderMap = new Dictionary<Vector2Int, PathfinderTile>();

            // Check test stuff.
            Assert.Throws<System.ArgumentException>(() => TilePathfinder.FindShortestPath(PathfinderMap, Vector2Int.zero, Vector2Int.zero, new LayerInfo(false, false)));
        }

        [UnityTest]
        public IEnumerator InvalidPositionsTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;
            int totalTiles = CountTiles(boardManager.FullMap);

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            // Check test stuff.
            Assert.Throws<System.ArgumentException>(() => TilePathfinder.FindShortestPath(PathfinderMap, Vector2Int.zero, new Vector2Int(400, 400), new LayerInfo(false, false)));
            Assert.Throws<System.ArgumentException>(() => TilePathfinder.FindShortestPath(PathfinderMap, new Vector2Int(-400, -400), Vector2Int.zero, new LayerInfo(false, false)));
            Assert.Throws<System.ArgumentException>(() => TilePathfinder.FindShortestPath(PathfinderMap, new Vector2Int(-400, -400), new Vector2Int(400, 400), new LayerInfo(false, false)));
        }

        [UnityTest]
        public IEnumerator ConnectNodeMapTest()
        {
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            // Check test stuff. Checking random tiles to ensure that they have the correct amount of connections.
            Assert.AreEqual(8, PathfinderMap[Vector2Int.zero].ConnectedTiles.Count);             // Middle tile
            Assert.AreEqual(3, PathfinderMap[new Vector2Int(-7, -5)].ConnectedTiles.Count);      // Corner tile
            Assert.AreEqual(3, PathfinderMap[new Vector2Int(6, 4)].ConnectedTiles.Count);        // Corner tile
            Assert.AreEqual(3, PathfinderMap[new Vector2Int(6, -5)].ConnectedTiles.Count);       // Corner tile
            Assert.AreEqual(5, PathfinderMap[new Vector2Int(-7, 0)].ConnectedTiles.Count);       // Side tile
            Assert.AreEqual(8, PathfinderMap[new Vector2Int(3, 3)].ConnectedTiles.Count);        // Middle tile
        }

        [UnityTest]
        public IEnumerator NoPathTest()
        {
            // Finds no valid path, (returns empty set).
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            List<Vector2> testpath = TilePathfinder.FindShortestPath(PathfinderMap, Vector2Int.zero, new Vector2Int(4, 4), new LayerInfo(false, false));

            Assert.NotNull(testpath);
            Assert.AreEqual(0, testpath.Count);
        }

        [UnityTest]
        public IEnumerator SimpleLineTest()
        {
            // Makes a simple straight line.
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            List<Vector2> testpath = TilePathfinder.FindShortestPath(PathfinderMap, Vector2Int.zero, new Vector2Int(0, 3), new LayerInfo(false, false));
            List<Vector2> expectedPath = new List<Vector2>();

            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(0, 1));
            expectedPath.Add(new Vector2(0, 2));
            expectedPath.Add(new Vector2(0, 3));

            Assert.NotNull(testpath);
            Assert.AreEqual(4, testpath.Count);
            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], testpath[i]);
                }
            }
            catch (System.Exception ex)
            {
                string str = PathToString(testpath); Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [UnityTest]
        public IEnumerator DiagonalLineTest()
        {
            // Makes a simple diagonal line.
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            List<Vector2> testpath = TilePathfinder.FindShortestPath(PathfinderMap, new Vector2Int(0, -5), new Vector2Int(-3, -2), new LayerInfo(false, false));
            List<Vector2> expectedPath = new List<Vector2>();

            expectedPath.Add(new Vector2(0, -5));
            expectedPath.Add(new Vector2(-1, -4));
            expectedPath.Add(new Vector2(-2, -3));
            expectedPath.Add(new Vector2(-3, -2));

            Assert.NotNull(testpath);
            Assert.AreEqual(4, testpath.Count);
            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], testpath[i]);
                }
            }
            catch (System.Exception ex)
            {
                string str = PathToString(testpath); Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [UnityTest]
        public IEnumerator SimpleBlockTest()
        {
            // Finds path around blocker.
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            List<Vector2> testpath = TilePathfinder.FindShortestPath(PathfinderMap, new Vector2Int(-5, -1), new Vector2Int(-2, -1), new LayerInfo(false, false));
            List<Vector2> expectedPath = new List<Vector2>();

            expectedPath.Add(new Vector2(-5, -1));
            expectedPath.Add(new Vector2(-4, -2));
            expectedPath.Add(new Vector2(-3, -2));
            expectedPath.Add(new Vector2(-2, -1));

            Assert.NotNull(testpath);
            Assert.AreEqual(4, testpath.Count);
            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], testpath[i]);
                }
            }
            catch (System.Exception ex)
            {
                string str = PathToString(testpath); Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }

        [UnityTest]
        public IEnumerator SimpleMazeTest()
        {
            // Finds path through simple maze.
            // Set up scene.
            SceneManager.LoadScene(TEST_SCENE);
            yield return new WaitWhile(() => SceneLoaded == false);
            SceneLoaded = false;    // Reset for other tests.

            // Set up test stuff.
            BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            IDictionary<Vector2Int, PathfinderTile> PathfinderMap;

            // Test Actions.
            PathfinderMap = TilePathfinder.BuildNodeMap(boardManager.FullMap, boardManager.MapLayerTiles);

            List<Vector2> testpath = TilePathfinder.FindShortestPath(PathfinderMap, new Vector2Int(-5, 3), new Vector2Int(4, -4), new LayerInfo(false, false));
            List<Vector2> expectedPath = new List<Vector2>();

            expectedPath.Add(new Vector2(-5, 3));
            expectedPath.Add(new Vector2(-4, 4));
            expectedPath.Add(new Vector2(-3, 4));
            expectedPath.Add(new Vector2(-2, 3));
            expectedPath.Add(new Vector2(-2, 2));
            expectedPath.Add(new Vector2(-1, 1));
            expectedPath.Add(new Vector2(0, 0));
            expectedPath.Add(new Vector2(1, -1));
            expectedPath.Add(new Vector2(2, -2));
            expectedPath.Add(new Vector2(3, -3));
            expectedPath.Add(new Vector2(4, -4));

            Assert.NotNull(testpath);
            Assert.AreEqual(11, testpath.Count);
            try
            {
                for (int i = 0; i < expectedPath.Count; i++)
                {
                    Assert.AreEqual(expectedPath[i], testpath[i]);
                }
            }
            catch (System.Exception ex)
            {
                string str = PathToString(testpath); Assert.Fail("ERROR: " + ex.ToString() + "\n" + str);
            }
        }
    }
}
