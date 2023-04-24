using UnityEngine;
using FroggyDefense.UI;

namespace FroggyDefense.Core
{
    /// <summary>
    /// Which kind of event for spawning the text.
    /// Used to determine which text properties should be used.
    /// </summary>
    public enum NumberPopupType
    {
        Damage,
        EnemyDefeated,
        GemPickup
    }

    public class NumberPopupManager : MonoBehaviour
    {
        [SerializeField] private bool _damageTextEnabled = true;
        [SerializeField] private bool _enemyDefeatedTextEnabled = true;
        [SerializeField] private bool _gemPickupTextEnabled = true;

        public GameObject m_DamageNumberTextPrefab;
        public GameObject m_EnemyDefeatedTextPrefab;
        public GameObject m_GemPickedUpTextPrefab;

        public Vector2 m_TextHorizontalOffsetRange = Vector2.zero;
        public Vector2 m_TextVerticalOffsetRange = Vector2.zero;

        // TODO: Add a way to track all text spawned by this and to clear it manually on an event (Boss intro or something).
        // TODO: Add a way to turn this off in settings. Set the Enabled bools.
        // TODO: Add a way to adjust how long numbers stay on the screen and how large they are in settings (Would change in PointsText).

        /// <summary>
        /// Spawns in a PointsText gameobject in the desired position.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="num"></param>
        private void SpawnText(GameObject prefab, Vector2 pos, float num)
        {
            pos.x += Random.Range(m_TextHorizontalOffsetRange.x, m_TextHorizontalOffsetRange.y);
            pos.y += Random.Range(m_TextVerticalOffsetRange.x, m_TextVerticalOffsetRange.y);

            var text = Instantiate(prefab, pos, Quaternion.identity).GetComponent<PointsText>();
            text.SetText(num);

            text.transform.SetParent(transform);
        }

        /// <summary>
        /// Lets the manager know to spawn in a PointsText gameobject.
        /// </summary>
        /// <param name="pos"></param>
        public void SpawnNumberText(Vector2 pos, float num, NumberPopupType type)
        {
            if (num < 0) return;

            switch (type)
            {
                case NumberPopupType.Damage:
                    if (_damageTextEnabled) SpawnText(m_DamageNumberTextPrefab, pos, num);
                    break;
                case NumberPopupType.EnemyDefeated:
                    if (_enemyDefeatedTextEnabled) SpawnText(m_EnemyDefeatedTextPrefab, pos, num);
                    break;
                case NumberPopupType.GemPickup:
                    if (_gemPickupTextEnabled) SpawnText(m_GemPickedUpTextPrefab, pos, num);
                    break;
                default:
                    Debug.LogWarning("NumberPopupManager could not render text. Unknown case.");
                    break;
            }
        }
    }
}