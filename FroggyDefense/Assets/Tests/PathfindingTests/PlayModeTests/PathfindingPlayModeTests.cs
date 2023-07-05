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
