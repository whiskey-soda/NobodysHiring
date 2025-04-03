using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDisplay : MonoBehaviour
{
    [SerializeField] Activity activity;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI minDurationText;
    [SerializeField] TextMeshProUGUI maxDurationText;
    [SerializeField] TextMeshProUGUI durationText;

    [Space]
    [SerializeField] Slider durationSlider;

    PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = activity.activityName;
        descriptionText.text = $"Description: {activity.description}";
        minDurationText.text = System.Math.Round(activity.minDuration,2).ToString();

        durationSlider.minValue = activity.minDuration;
        durationText.text = "Duration: " + System.Math.Round(durationSlider.value, 2).ToString();

        if (activity.maxDuration == 0)
        {
            float hoursUntilNoEnergy = playerStats.energy / activity.energyCost;
            durationSlider.maxValue = hoursUntilNoEnergy;
            maxDurationText.text = System.Math.Round(hoursUntilNoEnergy, 2).ToString();
        }
        else
        {
            durationSlider.maxValue = activity.maxDuration;
            maxDurationText.text = System.Math.Round(activity.maxDuration, 2).ToString();
        }
    }
}
