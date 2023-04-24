using UnityEngine;
using UnityEngine.UI;

namespace FroggyDefense.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Gradient gradient;               // The color gradient for the slider.
        public Image fill;                      // The health bar fill.
        public Image deltaTrace;                // The color showing if the health bar is increasing or decreasing.

        [SerializeField] private bool _traceHealth = true;
        public bool TraceHealth { get => _traceHealth; }         // If the health bar should animate the health tracing.

        [SerializeField] private float _traceTime = 1f;           // How fast the trace disappears.
        [SerializeField] private float _traceDelay = 1f;           // How long of a delay before the trace effect starts moving.
        public float TraceTime { get => _traceTime; set { _traceTime = value; } }
        public float TraceDelay { get => _traceDelay; set { _traceDelay = value; } }

        public Color decreasingColor;           // The color showing how much damage was lost.
        public Color increasingColor;             // The color showing how much health was gained.

        private float MaxHealth = 1f;
        private float Health = 1f;

        private float _startFillPercent = 1f;
        private float _targetFillPercent = 1f;
        private float _currTraceDelay = 1f;

        private bool tracing = false;

        private float TimeTracing = 0f;

        private Image MovingBar = null;         // The bar that steadily moves when tracing.

        // **************************************************
        // Update
        // **************************************************

        private void Update()
        {
            if (TraceHealth)
            {
                if (tracing)
                {
                    fill.color = gradient.Evaluate(fill.fillAmount); // Check what color it should be.

                    if (_currTraceDelay <= 0)
                    {
                        // Move the bar between the start and end points.
                        var num = Mathf.Lerp(_startFillPercent, _targetFillPercent, TimeTracing);
                        MovingBar.fillAmount = num;

                        TimeTracing += Time.deltaTime / _traceTime;

                        //Debug.Log("Tracing bar for " + gameObject.name + ".");

                        // Adjust to full value. Add an extra .01f to make sure there's no texture seams with the back bar.
                        if (TimeTracing >= 1)
                        {
                            tracing = false;
                            fill.fillAmount = _targetFillPercent + .01f;
                            deltaTrace.fillAmount = _targetFillPercent;
                            TimeTracing = 0;
                        }
                    } else
                    {
                        // Count down the delay to tracing.
                        _currTraceDelay -= Time.deltaTime;
                    }
                }
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        /// <summary>
        /// Instantly clears the bar.
        /// </summary>
        public void ClearBar()
        {
            fill.fillAmount = 0;
            if (deltaTrace != null) deltaTrace.fillAmount = 0;
            _targetFillPercent = 0;
            _currTraceDelay = 0;
            tracing = false;
        }

        /// <summary>
        /// Sets the bar to full at the given value.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        public void InitBar(float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.
            Health = maxHealth;
            _targetFillPercent = 1.0f * Health / MaxHealth;
            fill.fillAmount = _targetFillPercent;

            if (deltaTrace != null) deltaTrace.fillAmount = _targetFillPercent - .01f;

            fill.color = gradient.Evaluate(_targetFillPercent); // Check what color it should be.
        }

        /// <summary>
        /// Changes the new maximum health. Instantly adjusts the health bar.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.
            _targetFillPercent = 1.0f * Health / MaxHealth;
            fill.fillAmount = _targetFillPercent;

            if (deltaTrace != null) deltaTrace.fillAmount = _targetFillPercent - .01f;

            fill.color = gradient.Evaluate(_targetFillPercent); // Check what color it should be.
        }

        /// <summary>
        /// Instantly sets the health bar to the new health/maxHealth ratio.
        /// </summary>
        /// <param name="maxHealth"> The new max value </param>
        /// <param name="health"> The current value </param>
        public void SetMaxHealth(float health, float maxHealth)
        {
            MaxHealth = maxHealth;  // Set local maxHealth.
            SetHealth(health);
        }

        /// <summary>
        /// Changes the health value to the input value. If tracing is set, moves based on tracing speed.
        /// </summary>
        /// <param name="health"> The current value </param>
        public void SetHealth(float health)
        {
            _targetFillPercent = 1.0f * health / MaxHealth;

            // If tracing move different bars based on scenario.
            if (TraceHealth)
            {
                if (health >= Health)
                {
                    deltaTrace.color = increasingColor;
                    // Instantly move delta trace up.
                    deltaTrace.fillAmount = _targetFillPercent;
                    // Mark the fill as the moving bar.
                    MovingBar = fill;
                }
                else if (health < Health)
                {
                    deltaTrace.color = decreasingColor;
                    // Instantly move health up.
                    fill.fillAmount = _targetFillPercent;
                    // Mark deltaTrace as moving bar.
                    MovingBar = deltaTrace;
                }
                _currTraceDelay = _traceDelay;
                tracing = true;
                TimeTracing = 0;
                _startFillPercent = MovingBar.fillAmount;
            }
            else
            {
                // Move normally if not tracing.
                fill.fillAmount = _targetFillPercent;
                if (deltaTrace != null) deltaTrace.fillAmount = _targetFillPercent - .01f;
                fill.color = gradient.Evaluate(_targetFillPercent); // Check what color it should be.
            }
            Health = health;
        }
    }
}