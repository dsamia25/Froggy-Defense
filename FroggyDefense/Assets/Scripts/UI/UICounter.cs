using UnityEngine;
using TMPro;

namespace FroggyDefense.UI
{
    public class UICounter : MonoBehaviour
    {
        [Space]
        [Header("Text Settings")]
        [Space]
        [SerializeField] private TextMeshProUGUI m_Text = null;     // The text object to change.
        public string m_Header = "";                                // Header appended before the input string. (ex: ":\n").
        public bool m_HeaderOnSeparateLine = false;                 // If a new line should be included between the header and the input text.

        public float m_DefaultCount = 0;

        [Space]
        [Header("Scroll Settings")]
        [Space]
        public float m_IncreaseScrollTime = 1f;                     // How fast the number scrolls.
        public float m_DecreaseScrollTime = 1f;                     // How many seconds it takes to fully decrease
        public int m_MinimumScrollAmount = 4;                       // Minimum number change required to trigger a scroll effect.

        private float m_ScrollStartAmount = 0f;
        private float m_DisplayedAmount = 0f;
        private float m_TargetAmount = 0f;

        private bool increasing = false;
        private bool decreasing = false;

        private float TimeScrolling = 0f;

        private void Start()
        {
            UpdateText(m_DefaultCount);
        }

        private void Update()
        {
            if (increasing || decreasing)
            {
                var num = Mathf.Lerp(m_ScrollStartAmount, m_TargetAmount, TimeScrolling);
                SetText(num);

                if (increasing)
                {
                    TimeScrolling += Time.deltaTime / m_IncreaseScrollTime;
                }
                else if (decreasing)
                {
                    TimeScrolling += Time.deltaTime / m_DecreaseScrollTime;
                }

                if (TimeScrolling >= 1)
                {
                    increasing = false;
                    decreasing = false;
                    SetText(m_TargetAmount);
                }
            }
        }

        public void UpdateText(float _num)
        {
            UpdateText(Mathf.FloorToInt(_num));
        }

        public void UpdateText(int _num)
        {
            m_ScrollStartAmount = m_DisplayedAmount;
            m_TargetAmount = _num;

            // Check if the text should change instantly.
            if (Mathf.Abs(m_TargetAmount - m_DisplayedAmount) < m_MinimumScrollAmount)
            {
                SetText(m_TargetAmount);
                return;
            }

            if (m_TargetAmount < m_DisplayedAmount)
            {
                increasing = true;
                decreasing = false;
                TimeScrolling = 0;
            } else if (m_TargetAmount > m_DisplayedAmount)
            {
                increasing = false;
                decreasing = true;
                TimeScrolling = 0;
            }
        }

        private void SetText(float amount)
        {
            // Update the text.
            m_DisplayedAmount = Mathf.FloorToInt(amount);
            m_Text.text = m_Header + (m_HeaderOnSeparateLine ? "\n" : "") + m_DisplayedAmount;
        }
    }
}
