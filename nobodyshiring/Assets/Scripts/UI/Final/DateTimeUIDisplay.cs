using TMPro;
using UnityEngine;

public class DateTimeUIDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

    TimeTracking time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = TimeTracking.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        dateText.text = $"{months[time.month - 1]} {time.day}";

        // calculate in 24 hour time
        uint hour = (uint)Mathf.Floor(time.currentHour);
        uint minute = (uint)Mathf.FloorToInt((time.currentHour - hour) * 60);

        // convert to 12 hour time
        string ampm = "AM";
        if (hour > 12) { hour -= 12; ampm = "PM"; }
        else if (hour == 12) { ampm = "PM"; } // 12 noon
        else if (hour == 0) { hour = 12; } // 12 midnight

        timeText.text = $"{hour}:{minute.ToString().PadLeft(2, '0')} {ampm}";
    }
}
