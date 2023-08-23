using UnityEngine;

public enum DayPhase
{
    Dawn = 500,
    Day = 700,
    Dusk = 630,
    Night = 830
}

public class DayCycle : MonoBehaviour
{
    public static DayCycle instance;

    [SerializeField] private float _totalTime = 0;
    [SerializeField] private float _dayCycleTime = 2400;
    [SerializeField] private float timeSpeed = 1;

    [SerializeField] private Material DayCycleMaterial;         // The environment shader based on the day time.

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

        UpdateMaterials();
    }

    /// <summary>
    /// Sets the total time to the input time.
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        _totalTime = time;
    }

    /// <summary>
    /// Converts the DayPhase to its equivalent time and sets the time.
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(DayPhase time)
    {
        SetTime((float)time);
    }

    /// <summary>
    /// Updates materials dependent on the time of day. 
    /// </summary>
    private void UpdateMaterials()
    {
        DayCycleMaterial.SetFloat("DayPercent", StandardTime / DayCycleTime);
    }
}
