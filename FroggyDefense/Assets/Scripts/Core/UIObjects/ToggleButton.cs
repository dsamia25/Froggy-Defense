using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ToggleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ToggleButtonState[] states;

    private int stateIndex;
    public int StateIndex { get => stateIndex; set { stateIndex = value; UpdateUI(); } }
    public ToggleButtonState CurrentState => states[stateIndex];

    // TODO: Make an event when this is clicked to be listened to.
    public event Action ToggleEvent;

    private void Start()
    {
        UpdateUI();    
    }

    /// <summary>
    /// Updates the UI values to reflect the current state.
    /// </summary>
    public void UpdateUI()
    {
        if (states.Length > 0)
        {
            icon.sprite = CurrentState.sprite;
            icon.color = CurrentState.color;
            text.text = CurrentState.text;

            icon.gameObject.SetActive(CurrentState.sprite == null ? false : true);
            text.gameObject.SetActive(CurrentState.text.Equals("") ? false : true);
        }
    }

    /// <summary>
    /// Increments the state to the next in the cycle.
    /// </summary>
    public void IncrementState()
    {
        StateIndex = ++stateIndex % states.Length;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IncrementState();
        ToggleEvent?.Invoke();
    }

    [Serializable]
    public struct ToggleButtonState
    {
        public Sprite sprite;
        public Color color;
        public string text;

        public ToggleButtonState(Sprite _sprite, Color _color, string _text)
        {
            sprite = _sprite;
            color = _color;
            text = _text;
        }
    }
}
