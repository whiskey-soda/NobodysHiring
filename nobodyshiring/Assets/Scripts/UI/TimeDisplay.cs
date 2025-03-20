using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimeDisplay : MonoBehaviour
{
    TimeTracking time;

    TextMeshProUGUI timeText;

    private void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = TimeTracking.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate in 24 hour time
        uint hour = (uint)Mathf.Floor(time.currentHour);
        uint minute = (uint)Mathf.FloorToInt((time.currentHour - hour) * 60);

        // convert to 12 hour time
        string ampm = "am";
        if (hour > 12) { hour -= 12; ampm = "pm"; }
        else if (hour == 12) { ampm = "pm"; } // 12 noon
        else if ( hour == 0 ) { hour = 12; } // 12 midnight

        timeText.text = $"{hour}:{minute}{ampm}\n" +
            $"day: {time.day}\n" +
            $"month: {time.month}";
    }
}
