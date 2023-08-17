using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static DayCycle instance;

    [SerializeField] private float TotalTime = 0;
    [SerializeField] private float timeSpeed = 1;

    public float StandardTime;
    public int Hour;
    public int Minute;
    public int Second;

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
        TotalTime += timeSpeed * Time.deltaTime;
        StandardTime = TotalTime % 24;
        Second = Mathf.FloorToInt(TotalTime % 60);
        Minute = Mathf.FloorToInt((TotalTime / 60) % 60);
        Hour = Mathf.FloorToInt((TotalTime / 60 / 60) % 24);

    }
}
