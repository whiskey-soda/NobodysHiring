using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    // slider values are locked to whole numbers. values are converted to time values with the step size
    // (time * 60 / step size) converts from hours to slider values by converting hours to minutes, then dividing minutes by step size
    // (time * 60 / step size) converts from slider values to hours
    [SerializeField] float sliderStepSize = 5;

    Statbars statbars;
    PlayerStats stats;
    bool hovered = false;

    private void Awake()
    {
        nameTMP.text = activity.activityName;
        descriptionTMP.text = activity.description;
        minDurationTMP.text = ConvertToHourMinFormat(activity.minDuration);
        maxDurationTMP.text = ConvertToHourMinFormat(activity.maxDuration);

        durationSlider.minValue = activity.minDuration * (60 / sliderStepSize);
        durationSlider.maxValue = activity.maxDuration * (60 / sliderStepSize);

    }

    private void Start()
    {
        statbars = Statbars.Instance;
        stats = PlayerStats.Instance;

        // in start instead of awake because it relies on stat values, and stat singleton fetched in start
        DurationUpdated(durationSlider.value);
    }

    /// <summary>
    /// updates the display based on the duration chosen on the slider
    /// </summary>
    public void DurationUpdated(float newDuration)
    {
        // set text to display the prompt with chosen duration
        // value is multiplied by step size to convert from slider value to real time value
        buttonTMP.text = $"{activity.imperativeVerb} for {ConvertToHourMinFormat(newDuration / (60 / sliderStepSize))}";

        if (hovered) { ActivateStatPreview(); }
    }

    private void ActivateStatPreview()
    {
        statbars.energy.SetPreviewValue(stats.energy - activity.energyCost * durationSlider.value / (60 / sliderStepSize));
        statbars.motivation.SetPreviewValue(stats.motivation - activity.motivationCost * durationSlider.value / (60 / sliderStepSize));
    }

    /// <summary>
    /// converts a time in float (hours) to hour:minute format (xh ym)
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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hovered = true;

        ActivateStatPreview();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hovered = false;
    }
}
