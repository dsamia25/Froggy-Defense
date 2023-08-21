using UnityEngine;

public enum DayPhase
{
    Dawn,
    Day,
    Dusk,
    Night
}

public class DayCycle : MonoBehaviour
{
    public static DayCycle instance;

    [SerializeField] private float _totalTime = 0;
    [SerializeField] private float _dayCycleTime = 2400;
    [SerializeField] private float timeSpeed = 1;

    public DayPhase Phase;  // TODO: Make a way of changing this at time of the day.

    public float TotalTime => _totalTime;
    public float DayCycleTime => _dayCycleTime;
    public float StandardTime;
    public int Hour { get; private set; }
    public int Minute { get; private set; }
    public int Second { get; private set; }

    public Vector2Int DayTimeRange = new Vector2Int(7, 7);
    public bool IsDay => (Hour >= DayTimeRange.x && Hour <= DayTimeRange.y);
    public bool IsNight => !IsDay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogWarning($"Error: Already an instance of DayCycle.");
            Destroy(this);
        }
    }

    private void Update()
    {
        _totalTime += timeSpeed * Time.deltaTime;
        StandardTime = TotalTime % _dayCycleTime;
        Second = Mathf.FloorToInt(TotalTime % 60);
        Minute = Mathf.FloorToInt((TotalTime / 60) % 60);
        Hour = Mathf.FloorToInt((TotalTime / 60 / 60) % 24);
    }
}
