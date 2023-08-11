using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FroggyDefense.UI
{
    public class PointsText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Image m_Icon;

        public float m_RaiseTextHeight;
        public float m_RaiseTextTime;

        public float m_FadeTextDelay;
        public float m_FadeTextTime;

        public void SetText(int num)
        {
            m_Text.text = num.ToString();
        }

        public void SetText(float num)
        {
            m_Text.text = Mathf.Floor(num).ToString();
        }

        // TODO: This seems inefficient to keep reassigning a color variable when called from elsewhere instead of doing the loop in here.
        /// <summary>
        /// Sets the text opacity to the input percent between 0 and 1.
        /// </summary>
        /// <param name="fade"></param>
        public void SetFadePercent(float fade)
        {
            if (fade < 0) fade = 0;
            if (fade > 1) fade = 1;

            m_Text.alpha = fade;

            if (m_Icon != null)
            {
                Color color = m_Icon.color;
                color.a = fade;
                m_Icon.color = color;
            }
        }
    }
}
