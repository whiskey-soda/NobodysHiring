using UnityEngine;

public class TimeTracking : MonoBehaviour
{

    public float currentHour = 0;

    public static TimeTracking Instance;

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
        }
    }
}
