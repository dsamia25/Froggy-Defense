using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.UI
{
    public class ExperienceBarUpdater : MonoBehaviour
    {
        [SerializeField] private Character _player;
        [SerializeField] private HealthBar _xpBar;

        private void Awake()
        {
            // Subscribe to events.
            _player.CharacterExperienceChanged += UpdateBar;
        }

        private void Start()
        {
            // Initialize experience bar.
            _xpBar.SetMaxHealth(_player.Xp, _player.XpNeeded);
        }

        /// <summary>
        /// Called on event for when the player's experience changes. Updates the healthbar.
        /// </summary>
        private void UpdateBar()
        {
            _xpBar.SetMaxHealth(_player.Xp, _player.XpNeeded);
        }
    }
}
