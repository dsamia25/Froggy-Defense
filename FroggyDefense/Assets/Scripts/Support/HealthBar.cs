using UnityEngine;
using UnityEngine.UI;

namespace FroggyDefense.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Gradient gradient;               // The color gradient for the slider.
        public Image fill;                      // The health bar fill.
        public Image deltaTrace;                // The color showing if the health bar is increasing or decreasing.
        public bool TraceHealth = true;         // If the health bar should animate the health tracing.
        public float traceSpeed = 1f;           // How fast the trace disappears.
        public float traceDelay = 1f;           // How long of a delay before the trace effect starts moving.
        public Color traceDecreaseColor;        // The color showing how much damage was lost.
        public Color traceIncreaseColor;        // The color showing how much health was gained.

        private float MaxHealth = 1f;
        private float Health = 1f;

        private float targetFillPercent = 1f;
        private float currTraceDelay = 1f;
        private bool traceIsMoving = false;

        // **************************************************
        // Update
        // **************************************************

        private void Update()
        {
            if (TraceHealth)
            {
                if (currTraceDelay <= 0)
                {
                    if (deltaTrace.fillAmount >= targetFillPercent)
                    {
                        traceIsMoving = true;
                        deltaTrace.fillAmount -= traceSpeed * Time.deltaTime;
                    }
                    else
                    {
                        traceIsMoving = false;
                    }
                }
                else
                {
                    traceIsMoving = false;
                    currTraceDelay -= Time.deltaTime;
                }
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        /// <summary>
        /// Changes the new maximum health.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        /// <param name="health"> The current value </param>
        public void SetMaxHealth(float health, float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.
            Health = health;

            if (!traceIsMoving) currTraceDelay = traceDelay;

            targetFillPercent = 1.0f * health / MaxHealth;
            fill.fillAmount = targetFillPercent;

            fill.color = gradient.Evaluate(targetFillPercent >= 0 ? targetFillPercent : 0); // Check what color it should be.
        }

        /// <summary>
        /// Changes the new maximum health.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.

            if (!traceIsMoving) currTraceDelay = traceDelay;

            targetFillPercent = 1.0f * Health / MaxHealth;
            fill.fillAmount = targetFillPercent;

            fill.color = gradient.Evaluate(targetFillPercent); // Check what color it should be.
        }

        /// <summary>
        /// Changes the health value to the input value.
        /// </summary>
        /// <param name="health"> The current value </param>
        public void SetHealth(float health)
        {
            deltaTrace.color = health < Health ? traceDecreaseColor : traceIncreaseColor;     // Adjust backing color.

            Health = health;

            if (!traceIsMoving) currTraceDelay = traceDelay;

            targetFillPercent = 1.0f * health / MaxHealth;
            fill.fillAmount = targetFillPercent;

            fill.color = gradient.Evaluate(targetFillPercent); // Check what color it should be.
        }
    }
}