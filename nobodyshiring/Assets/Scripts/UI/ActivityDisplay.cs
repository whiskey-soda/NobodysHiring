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
        //durationText.text = $"Duration: {activity.duration} hours";
        descriptionText.text = $"Description: {activity.description}";
        minDurationText.text = activity.minDuration.ToString();

        durationSlider.minValue = activity.minDuration;
        durationText.text = "Duration: " + durationSlider.value.ToString();

        if (activity.maxDuration == 0)
        {
            float hoursUntilNoEnergy = playerStats.energy / activity.energyCost;
            durationSlider.maxValue = hoursUntilNoEnergy;
            maxDurationText.text = hoursUntilNoEnergy.ToString();
        }
        else
        {
            durationSlider.maxValue = activity.maxDuration;
            maxDurationText.text = activity.maxDuration.ToString();
        }
    }
}
