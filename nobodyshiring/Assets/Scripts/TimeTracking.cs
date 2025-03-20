using UnityEngine;
using UnityEngine.Events;

public class TimeTracking : MonoBehaviour
{

    public float currentHour = 0;
    public uint day { get; private set; } = 1;
    public uint month { get; private set; } = 1;

    float daysInMonth = 30;

    public UnityEvent dayEnd;
    public UnityEvent monthEnd;

    public static TimeTracking Instance;

    MotivationDrain motivationDrain;

    private void Start()
    {
        motivationDrain = MotivationDrain.Instance;
    }

    private void Awake()
    {
        // singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void PassTime(float time)
    {
        currentHour += time;
        if (currentHour > 24)
        {
            currentHour = currentHour - 24;
            IncrementDay();
        }

        // apply passive motivation drain per hour passed
        motivationDrain.ApplyMotivationDrain(time);

    }

    void IncrementDay()
    {
        day ++;
        if (day > daysInMonth)
        {
            IncrementMonth();
            day = 1;
        }

        dayEnd.Invoke();
    }

    void IncrementMonth()
    {
        month++;
        if (month > 12)
        { 
            month = 1;
        }

        monthEnd.Invoke();
    }

}
