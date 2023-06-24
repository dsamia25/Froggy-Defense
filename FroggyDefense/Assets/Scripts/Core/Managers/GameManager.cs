using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core.Buildings;
using FroggyDefense.Core.Enemies;

namespace FroggyDefense.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GemManager _gemManager;
        [SerializeField] private BoardManager _boardManager;
        [SerializeField] private NumberPopupManager _numberPopupManager;
        [SerializeField] private UiManager _uiManager;

        public static GameManager instance;
        public GemManager m_GemManager { get => _gemManager; }
        public BoardManager BoardManager { get => _boardManager; }
        public NumberPopupManager m_NumberPopupManager { get => _numberPopupManager; }
        public UiManager m_UiManager { get => _uiManager; }
        public Spawner spawner;
        public GameObject m_DamageAreaPrefab;
        public Player m_Player;

        public static bool GameStarted { get; private set; }        // If the game has been started.
        public static bool ShootingEnabled { get; private set; }    // If anything is currently allowed to shoot.

        [Space]
        [Header("Level Settings")]
        [Space]
        [SerializeField] private int WavesPerMapExpansion = 3;

        [Space]
        [Header("Points")]
        [Space]
        [SerializeField] private int _points = 0;
        public int Points {
            get => _points;
            private set
            {
                if (value < 0)
                {
                    _points = 0;
                }
                _points = value;
                PointsChangedEvent?.Invoke(_points);
            }
        }   // The amount of points the player has.

        [Space]
        [Header("Wave Info")]
        [Space]
        [SerializeField] private int _waveNumber = 0;
        [SerializeField] private bool _waveActive = false;
        public int WaveNumber { get => _waveNumber; private set => _waveNumber = value; }
        public bool WaveActive { get => _waveActive; private set { _waveActive = value; } }

        [Space]
        [Header("Enemies")]
        [Space]
        [SerializeField] private int _enemiesThisWave = 0;
        [SerializeField] private int _activeEnemies = 0;
        public int EnemiesThisWave { get => _enemiesThisWave; private set { _enemiesThisWave = value; } }   // How many enemies will spawn this wave.
        public int ActiveEnemies { get => _activeEnemies; private set { _activeEnemies = value; } }         // How many enemies are currently alive.
        public List<Enemy> Enemies = new List<Enemy>();                                                     // List of all enemies on the map.
        public List<Spawner> Spawners = new List<Spawner>();                                                // List of all spawners on the map.

        [Space]
        [Header("Events")]
        [Space]
        //public UnityEvent WaveStartedEvent;
        //public UnityEvent WaveCompletedEvent;
        public UnityEvent<int> PointsChangedEvent;
        //public UnityEvent RefreshUIEvent;

        public delegate void GameDelegate();
        public static event GameDelegate WaveStartedEvent;
        public static event GameDelegate WaveCompletedEvent;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one GameManager");
                return;
            }
            instance = this;    // Set singleton

            if (_boardManager == null)
            {
                _boardManager = gameObject.GetComponent<BoardManager>();
            }

            if (_gemManager == null)
            {
                _gemManager = gameObject.GetComponent<GemManager>();
            }

            if (_numberPopupManager == null)
            {
                _numberPopupManager = gameObject.GetComponent<NumberPopupManager>();
            }

            // Set default values.
            GameStarted = true;
            ShootingEnabled = false;
        }

        private void Start()
        {
            Enemy.EnemyDefeatedEvent += OnEnemyDefeatedEvent;
        }

        /// <summary>
        /// Marks the start a new game.
        /// </summary>
        public void StartGame()
        {
            GameStarted = true;
            ShootingEnabled = true;
        }

        /// <summary>
        /// Starts spawning a new wave.
        /// </summary>
        public void StartWave()
        {
            SetEnemiesThisWave();
            WaveActive = true;
            WaveStartedEvent?.Invoke();
        }

        /// <summary>
        /// Resets wave values and invokes wave completed event.
        /// </summary>
        public void WaveCompleted()
        {
            Debug.Log("Wave " + WaveNumber + " Completed!");

            WaveActive = false;
            WaveNumber++;
            ActiveEnemies = 0;
            EnemiesThisWave = 0;
            WaveCompletedEvent?.Invoke();

            if (WaveNumber % WavesPerMapExpansion == 0)
            {
                Debug.Log("Expanding Level");
                _boardManager?.ExpandLevel();
            }

        }

        /// <summary>
        /// Marks the 
        /// </summary>
        public void GameOver()
        {

        }

        /// <summary>
        /// Decrements the amount of active enemies this wave.
        /// </summary>
        public void OnEnemyDefeatedEvent(EnemyEventArgs args)
        {
            Points += args.points;      // Earn Points.
            ActiveEnemies--;            // Decrement enemy count.
            EnemiesThisWave--;          // Decrement enemy count.

            // Check if wave is over.
            if (ActiveEnemies <= 0 && EnemiesThisWave <= 0)
            {
                WaveCompleted();
            }
            Debug.Log("Enemy Defeated. Active Enemies: " + _activeEnemies);
        }

        private int SpawnPowerup()
        {
            return -1;
        }

        /// <summary>
        /// Adjusts the amount of enemies left this wave.
        /// </summary>
        private void SetEnemiesThisWave()
        {
            foreach (Spawner spawner in Spawners)
            {
                EnemiesThisWave += Mathf.FloorToInt(spawner.SpawnCount);
            }
        }
    }
}