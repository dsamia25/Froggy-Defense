using System.Collections;
using UnityEngine;
using TMPro;

namespace FroggyDefense.UI
{
    public class UICounter : MonoBehaviour
    {
        [Space]
        [Header("Text")]
        [Space]
        [SerializeField] private TextMeshProUGUI m_Text = null;     // The text object to change.
        public string m_Header = "";                                // Header appended before the input string. (ex: ":\n").
        public bool m_HeaderOnSeparateLine = false;                 // If a new line should be included between the header and the input text.

        public float m_DefaultCount = 0;

        [Space]
        [Header("Count")]
        [Space]
        public float m_numScrollSpeed = 2;                 // How fast the number scrolls.
        public int m_MinimumScrollAmount = 4;                       // Minimum number change required to trigger a scroll effect.

        private float m_DisplayedAmount = 0f;
        private float m_numStartAmount = 0f;
        private float m_numTargetAmount = 0f;
        private bool m_ScrollingText = false;

        private void Start()
        {
            UpdateText(m_DefaultCount);
        }

        public void UpdateText(float _num)
        {
            UpdateText(Mathf.FloorToInt(_num));
        }

        public void UpdateText(int _num)
        {
            m_numStartAmount = m_DisplayedAmount;
            m_numTargetAmount = _num;

            if (Mathf.Abs(m_numTargetAmount - m_numStartAmount) < m_MinimumScrollAmount && !m_ScrollingText)
            {
                m_DisplayedAmount = m_numTargetAmount;

                // Update the text.
                m_Text.text = m_Header + (m_HeaderOnSeparateLine ? "\n" : "") + Mathf.FloorToInt(m_DisplayedAmount);
                return;
            }

            if (!m_ScrollingText)
            {
                m_ScrollingText = true;
                StartCoroutine(UpdateTextCoroutine());
            }
        }

        private IEnumerator UpdateTextCoroutine()
        {
            float i = 0;
            while (i <= 1)
            {
                m_DisplayedAmount = Mathf.Lerp(m_numStartAmount, m_numTargetAmount, i);
                i += Time.deltaTime / m_numScrollSpeed;

                // Update the text.
                m_Text.text = m_Header + (m_HeaderOnSeparateLine ? "\n" : "") + Mathf.FloorToInt(m_DisplayedAmount);
                yield return null;
            }
            m_DisplayedAmount = m_numTargetAmount;

            // Update the text.
            m_Text.text = m_Header + (m_HeaderOnSeparateLine ? "\n" : "") + Mathf.FloorToInt(m_DisplayedAmount);

            m_ScrollingText = false;
        }
    }
}
