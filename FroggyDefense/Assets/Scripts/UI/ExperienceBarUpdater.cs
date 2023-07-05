using UnityEngine;
using FroggyDefense.Core;
using System.Collections;

namespace FroggyDefense.UI
{
    public class ExperienceBarUpdater : MonoBehaviour
    {
        [SerializeField] private Character _player;
        [SerializeField] private HealthBar _xpBar;

        private void Awake()
        {
            // Subscribe to events.
            _player.CharacterExperienceChanged += OnXpGainedEvent;
            _player.CharacterLeveledUp += OnLeveledUpEvent;
        }

        private void Start()
        {
            if (_player != null && _xpBar != null)
            {
                // Initialize experience bar.
                _xpBar.SetMaxHealth(_player.Xp, _player.XpNeeded);
            }
        }

        /// <summary>
        /// Called on event for when the player's experience changes. Updates the healthbar.
        /// </summary>
        private void OnXpGainedEvent()
        {
            _xpBar.SetHealth(_player.Xp);
        }

        private void OnLeveledUpEvent()
        {
            StartCoroutine(LevelUpCoroutine());
        }

        private IEnumerator LevelUpCoroutine()
        {
            // Start filling bar.
            _xpBar.SetHealth(_player.Xp);
            // Wait for time to fill bar.
            yield return new WaitForSeconds(1.05f * (_xpBar.TraceTime + _xpBar.TraceDelay));
            // Reset bar to 0.
            _xpBar.ClearBar();
            // Fill bar to amount.
            _xpBar.SetMaxHealth(_player.Xp, _player.XpNeeded);
            yield return null;
        }
    }
}
