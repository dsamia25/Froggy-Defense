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
        public Color movingDownColor;        // The color showing how much damage was lost.
        public Color movingUpColor;        // The color showing how much health was gained.

        private float MaxHealth = 1f;
        private float Health = 1f;

        private float targetFillPercent = 1f;
        private float currTraceDelay = 1f;
        public bool traceIsMoving = false;
        private bool movingDown = false;
        private bool movingUp = false;

        private Image MovingBar = null;         // The bar that steadily moves when tracing.

        // **************************************************
        // Update
        // **************************************************

        private void Update()
        {
            if (TraceHealth)
            {
                if (currTraceDelay <= 0)
                {
                    if (movingDown)
                    {
                        //Debug.Log("Moving Down");
                        if (MovingBar.fillAmount >= targetFillPercent)
                        {
                            traceIsMoving = true;   // Make sure is still moving.
                            MovingBar.fillAmount -= traceSpeed * Time.deltaTime;   // Update fill.
                        }
                        else
                        {
                            //Debug.Log("Resetting move down");

                            // Reset bools.
                            traceIsMoving = false;
                            movingDown = false;
                            MovingBar.fillAmount = targetFillPercent + .01f;
                        }
                        fill.color = gradient.Evaluate(targetFillPercent >= 0 ? targetFillPercent : 0); // Check what color it should be.
                    }
                    else if (movingUp)
                    {
                        //Debug.Log("Moving Up");
                        if (MovingBar.fillAmount <= targetFillPercent)
                        {
                            traceIsMoving = true;   // Make sure is still moving.
                            MovingBar.fillAmount += traceSpeed * Time.deltaTime;   // Update fill.
                        }
                        else
                        {
                            //Debug.Log("Resetting move up");

                            // Reset bools.
                            traceIsMoving = false;
                            movingUp = false;
                            MovingBar.fillAmount = targetFillPercent + .01f;
                        }
                        fill.color = gradient.Evaluate(targetFillPercent >= 0 ? targetFillPercent : 0); // Check what color it should be.
                    }
                }
                else
                {
                    // Reset bools.
                    traceIsMoving = false;

                    // Count down on delay.
                    currTraceDelay -= Time.deltaTime;
                }
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        /// <summary>
        /// Instantly sets the health bar to the new health/maxHealth ratio.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        /// <param name="health"> The current value </param>
        public void SetMaxHealth(float health, float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.
            Health = health;

            if (MaxHealth == 0) MaxHealth = 1;

            targetFillPercent = 1.0f * health / MaxHealth;
            fill.fillAmount = targetFillPercent;

            if (deltaTrace != null) deltaTrace.fillAmount = targetFillPercent - .01f;

            fill.color = gradient.Evaluate(targetFillPercent >= 0 ? targetFillPercent : 0); // Check what color it should be.
        }

        /// <summary>
        /// Changes the new maximum health. Instantly adjusts the health bar.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.

            targetFillPercent = 1.0f * Health / MaxHealth;
            fill.fillAmount = targetFillPercent;

            if (deltaTrace != null) deltaTrace.fillAmount = targetFillPercent - .01f;

            fill.color = gradient.Evaluate(targetFillPercent); // Check what color it should be.
        }

        /// <summary>
        /// Changes the health value to the input value. If tracing is set, moves based on tracing speed.
        /// </summary>
        /// <param name="health"> The current value </param>
        public void SetHealth(float health)
        {
            targetFillPercent = 1.0f * health / MaxHealth;

            // If tracing move different bars based on scenario.
            if (TraceHealth)
            {
                if (health > Health)
                {
                    // Moving up.
                    deltaTrace.color = movingUpColor;

                    // Instantly move delta trace up.
                    deltaTrace.fillAmount = targetFillPercent;

                    // Mark the fill as the moving bar.
                    MovingBar = fill;

                    movingDown = false;
                    movingUp = true;
                }
                else if (health < Health)
                {
                    // Moving down.
                    deltaTrace.color = movingDownColor;

                    // Instantly move health up.
                    fill.fillAmount = targetFillPercent;

                    // Mark deltaTrace as moving bar.
                    MovingBar = deltaTrace;

                    movingDown = true;
                    movingUp = false;
                }
                traceIsMoving = true;
                currTraceDelay = traceDelay;
            }
            else
            {
                // Move normally if not tracing.
                fill.fillAmount = targetFillPercent;
                if (deltaTrace != null) deltaTrace.fillAmount = targetFillPercent - .01f;
                fill.color = gradient.Evaluate(targetFillPercent); // Check what color it should be.
            }

            Health = health;
        }
    }
}