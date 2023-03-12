using UnityEngine;
using FroggyDefense.Interactables;
using FroggyDefense.Core;

namespace FroggyDefense.UI
{
    public class PointsRenderer : MonoBehaviour
    {
        public GameObject m_DamageNumberTextPrefab;
        public GameObject m_EnemyDefeatedTextPrefab;
        public GameObject m_GemPickedUpTextPrefab;

        public Vector2 m_TextHorizontalOffsetRange = Vector2.zero;
        public Vector2 m_TextVerticalOffsetRange = Vector2.zero;

        // TODO: Add a way to track all text spawned by this and to clear it manually on an event (Boss intro or something).
        // TODO: Add a way to turn this off in settings.
        // TODO: Add a way to adjust how long numbers stay on the screen and how large they are in settings (Would change in PointsText).

        private void Start()
        {
            Enemy.EnemyDamagedEvent += OnEnemyDamagedEvent;
            Enemy.EnemyDefeatedEvent += OnEnemyDefeatedEvent;
            Gem.PickedUpEvent += OnGemPickedUpEvent;
        }

        private void OnEnemyDamagedEvent(EnemyEventArgs args)
        {
            if (args.damage < 0) return;

            SpawnText(m_DamageNumberTextPrefab, args.pos, args.damage);
        }

        private void OnEnemyDefeatedEvent(EnemyEventArgs args)
        {
            if (args.points < 0) return;

            SpawnText(m_EnemyDefeatedTextPrefab, args.pos, args.points);
        }

        private void OnGemPickedUpEvent(GemEventArgs args)
        {
            if (args.value < 0) return;

            SpawnText(m_GemPickedUpTextPrefab, args.pos, args.value);
        }

        private void SpawnText(GameObject prefab, Vector2 pos, float num)
        {
            pos.x += Random.Range(m_TextHorizontalOffsetRange.x, m_TextHorizontalOffsetRange.y);
            pos.y += Random.Range(m_TextVerticalOffsetRange.x, m_TextVerticalOffsetRange.y);

            var text = Instantiate(prefab, pos, Quaternion.identity).GetComponent<PointsText>();
            text.SetText(num);

            text.transform.SetParent(transform);
        }
    }
}