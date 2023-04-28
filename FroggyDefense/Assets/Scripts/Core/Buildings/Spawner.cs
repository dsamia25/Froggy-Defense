using System.Collections;
using UnityEngine;

namespace FroggyDefense.Core.Buildings
{
    public class Spawner : MonoBehaviour
    {
        public GameObject _prefab;

        public float SpawnCount;                        // How many minions this spawner spawns.
        public float SpawnDelay;                        // The delay between each minion spawning.

        public float SpawnCountIncreasePerWave = .5f;   // How many more minions this spawner will spawn each turn.

        private float _spawnsLeft = 0;                   // How many more enemies to spawn this wave.

        private void Start()
        {
            // Subscribe to events.
            GameManager.instance.WaveStartedEvent.AddListener(OnWaveStartedEvent);
            GameManager.instance.WaveCompletedEvent.AddListener(OnWaveCompletedEvent);
        }

        /// <summary>
        /// Spawns a single object at the spawner's position.
        /// </summary>
        public void Spawn()
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Spawns a series of objects at the spawner's position using its preset values.
        /// </summary>
        public void SpawnSeries()
        {
            SpawnSeries(Mathf.FloorToInt(SpawnCount), SpawnDelay);
        }

        /// <summary>
        /// Spawns a series of objects at the spawner's position using input values.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        public void SpawnSeries(int count, float delay)
        {
            if (_prefab == null) return;
            if (count <= 0) return;
            if (delay < 0) return;
            _spawnsLeft = count;

            if (count <= 1)
            {
                Spawn();
                _spawnsLeft--;
                return;
            }

            StartCoroutine(SpawnRoutine(count, delay));
        }

        /// <summary>
        /// Spawns an input number of objects with a delay.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator SpawnRoutine(int count, float delay)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
                yield return new WaitForSeconds(delay);
            }
        }

        public void OnWaveCompletedEvent()
        {
            SpawnCount += SpawnCountIncreasePerWave;
        }

        public void OnWaveStartedEvent()
        {
            SpawnSeries();
        }
    }
}