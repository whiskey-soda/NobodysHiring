using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Date
{
    public uint month;
    public uint day;

    public Date(uint month, uint day)
    {
        this.month = month;
        this.day = day;
    }

    /// <summary>
    /// comparison method, returns true if both dates are the same.
    /// if month is 0, then only the date is considered
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Date other)
    {
        // if month is 0, means the month doesnt matter. only look at the day.
        // used for monthly due dates and stuff
        if ((other.month == month || other.month == 0)
            && other.day == day) { return true; }
        else { return false; }
    }

    public void MoveDate(uint days)
    {
        for (int i = 0; i < Mathf.Abs(days); i++)
        {
            // increase day value into future
            if (Mathf.Sign(days) > 0) { day++; }
            // decrease day value into past
            else if (Mathf.Sign(days) < 0) {  day--; }

            // set to max day value. decrement month
            if (day < 1){ day = TimeTracking.Instance.daysInMonth; month--; }
            // set to minimum day value. increment month
            else if (day > TimeTracking.Instance.daysInMonth) { day = 1; month++; }
        }
    }

    public override string ToString()
    {
        return $"{month} / {day}";
    }
}

public class TimeTracking : MonoBehaviour
{

    public float currentHour { get; private set; } = 7.5f;
    public uint day { get; private set; } = 2;
    public uint month { get; private set; } = 1;

    public uint daysInMonth { get; private set; } = 30;

    public UnityEvent dayEnd;
    public UnityEvent monthEnd;

    public static TimeTracking Instance;

    MotivationDrain motivationDrain;
    Thermostat thermostat;
    ExpensesController expensesController;

    private void Start()
    {
        motivationDrain = MotivationDrain.Instance;
        thermostat = Thermostat.Instance;
        expensesController = ExpensesController.Instance;
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

        // add thermostat running time, if needed
        if (thermostat.isOn) { expensesController.UseThermostat(time); }
    }

    void IncrementDay()
    {
        day++;
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

    public Date GetCurrentDate()
    {
        return new Date(month, day);
    }

}
