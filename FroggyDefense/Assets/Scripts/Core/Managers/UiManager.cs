using System.Collections;
using UnityEngine;
using TMPro;
using FroggyDefense.Core.Buildings.UI;

namespace FroggyDefense.Core
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI m_GemsText;
        public TextMeshProUGUI m_PointsText;

        public TurretSheetUI m_TurretSheetUi;

        public float m_TextScrollIncrementSpeed = 2;    // How many 
        public int m_MinimumGemScrollAmount = 4;        // Minimum gem change required to trigger a scroll effect.

        private float m_DisplayedGemsAmount = 0f;       // Gems
        private float m_GemsTextStartAmount = 0f;       // Gems
        private float m_GemsTextTargetAmount = 0f;      // Gems
        private float m_DisplayedPointsAmount = 0f;     // Points
        private float m_PointsTextStartAmount = 0f;     // Points
        private float m_PointsTextTargetAmount = 0f;    // Points

        private bool m_ScrollingGemsText = false;       // Gems
        private bool m_ScrollingPointsText = false;     // Points

        /// <summary>
        /// Increments the gems text.
        /// </summary>
        public void UpdateGemsText()
        {
            m_GemsTextStartAmount = m_DisplayedGemsAmount;
            m_GemsTextTargetAmount = GameManager.instance.m_GemManager.Gems;

            if (Mathf.Abs(m_GemsTextTargetAmount - m_GemsTextStartAmount) < m_MinimumGemScrollAmount && !m_ScrollingGemsText)
            {
                m_DisplayedGemsAmount = m_GemsTextTargetAmount;

                // Update the text.
                m_GemsText.text = "Gems:\n" + Mathf.FloorToInt(m_DisplayedGemsAmount);
                return;
            }

            if (!m_ScrollingGemsText)
            {
                m_ScrollingGemsText = true;
                StartCoroutine(UpdateGemsTextCoroutine());
            }
        }

        private IEnumerator UpdateGemsTextCoroutine()
        {
            float i = 0;
            while (i <= 1) {
                m_DisplayedGemsAmount = Mathf.Lerp(m_GemsTextStartAmount, m_GemsTextTargetAmount, i);
                i += Time.deltaTime / m_TextScrollIncrementSpeed;

                // Update the text.
                m_GemsText.text = "Gems:\n" + Mathf.FloorToInt(m_DisplayedGemsAmount);
                yield return null;
            }
            m_DisplayedGemsAmount = m_GemsTextTargetAmount;

            // Update the text.
            m_GemsText.text = "Gems:\n" + Mathf.FloorToInt(m_DisplayedGemsAmount);

            m_ScrollingGemsText = false;
        }

        /// <summary>
        /// Increments the points text.
        /// </summary>
        public void UpdatePointsText()
        {
            // TODO: Increments the points text.
        }

        private IEnumerator UpdatePointsTextCoroutine()
        {
            float i = 0;
            while (i <= 1)
            {
                m_DisplayedGemsAmount = Mathf.Lerp(m_GemsTextStartAmount, m_GemsTextTargetAmount, i);
                i += Time.deltaTime / m_TextScrollIncrementSpeed;

                // Update the text.
                m_GemsText.text = "Gems:\n" + Mathf.FloorToInt(m_DisplayedGemsAmount);
                yield return null;
            }
            m_DisplayedGemsAmount = m_GemsTextTargetAmount;

            // Update the text.
            m_GemsText.text = "Gems:\n" + Mathf.FloorToInt(m_DisplayedGemsAmount);

            m_ScrollingGemsText = false;
        }
    }
}