using System.Collections;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Interactables;

namespace FroggyDefense
{
    public class GemDropper : MonoBehaviour, IDropper
    {
        [Space]
        [Header("Gems")]
        [Space]
        public int DropAmount = 0;

        public float m_DropTimeSpacing = .1f;   // The time between each drop.
        public float m_DropForce = 1f;          // The force pushing each item when it is dropped.

        /// <summary>
        /// Drops gems.
        /// </summary>
        public void Drop()
        {
            StartCoroutine(DropSequence());
        }

        /// <summary>
        /// Drops gems.
        /// </summary>
        public void Drop(int amount)
        {
            Debug.Log("GemDropper dropping " + amount + " gems.");
            DropAmount = amount;
            StartCoroutine(DropSequence());
        }

        private IEnumerator DropSequence()
        {
            int amount = DropAmount;

            GemValueChartObject chart = GameManager.instance.m_GemManager.GemChart;
            int currValue = 0;
            for (int i = chart.GemTypes - 1; i >= 0; i--)
            {
                currValue = chart.Gems[i].GemValue;
                int currGem = amount / currValue;
                amount -= currGem * currValue;

                for (int j = 0; j < currGem; j++)
                {
                    GameObject gem = Instantiate(chart.BaseGemPrefab, transform.position, Quaternion.identity);
                    var gemComp = gem.GetComponent<Gem>();
                    gemComp.SetCurrency(GameManager.instance.m_GemManager.GemCurrencyObject);
                    gemComp.SetGem(chart.Gems[i]);
                    gemComp.Launch(m_DropForce * GetRandomAngle());
                }
                yield return new WaitForSeconds(m_DropTimeSpacing);
            }
            Destroy(gameObject);
            yield return null;
        }

        private Vector2 GetRandomAngle()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}
