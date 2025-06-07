using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityPanel : MonoBehaviour
{
    [SerializeField] Activity activity;
    [Space]
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI descriptionTMP;
    [SerializeField] TextMeshProUGUI minDurationTMP;
    [SerializeField] TextMeshProUGUI maxDurationTMP;
    [Space]
    [SerializeField] TextMeshProUGUI buttonTMP;
    [SerializeField] Slider durationSlider;

    private void Awake()
    {
        nameTMP.text = activity.activityName;
        descriptionTMP.text = activity.description;
        minDurationTMP.text = ConvertToHourMinFormat(activity.minDuration);
        maxDurationTMP.text = ConvertToHourMinFormat(activity.maxDuration);

        durationSlider.minValue = activity.minDuration;
        durationSlider.maxValue = activity.maxDuration;
    }

    // Update is called once per frame
    void Update()
    {
         buttonTMP.text = $"{activity.imperativeVerb} for {ConvertToHourMinFormat(durationSlider.value)}";
    }

    /// <summary>
    /// converts a time in float to hour:minute format (xh ym)
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private string ConvertToHourMinFormat(float duration)
    {
        string hourMinFormatString = "";

        // get hour by truncating float with an int cast
        if ((int)duration > 0) { hourMinFormatString += $"{(int)duration}h "; }

        // get minute by taking the decimal portion and multiplying by 60 and then truncating off the decimal with an int cast
        hourMinFormatString += $"{(int)((duration - (int)duration) * 60)}m";

        return hourMinFormatString;
    }
}
