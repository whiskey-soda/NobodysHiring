using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GigDisplay : MonoBehaviour
{
    [SerializeField] Gig gig;
    [Space]

    [SerializeField] Button workButton;
    [Space]

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI payText;
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
        nameText.text = $"{gig.gigName}: {gig.currentStage.taskName}";
        progressText.text = $"Progress: {System.Math.Round(gig.currentStage.currentProgress, 2)} / {gig.currentStage.progressMax} | " +
            $"Stage: {gig.stages.IndexOf(gig.currentStage)+1} / {gig.stages.Count}";
        payText.text = $"Current Stage Pay: ${gig.currentStage.pay}";

        minDurationText.text = "0";
        durationText.text = "Duration: " + System.Math.Round(durationSlider.value, 2).ToString();

        // set max duration on the slider to maximum hours before passing out
        /*float hoursUntilNoEnergy = playerStats.energy / gig.currentStage.energyCostPerHour;

        durationSlider.maxValue = hoursUntilNoEnergy;
        maxDurationText.text = System.Math.Round(hoursUntilNoEnergy, 2).ToString();
        */
    }
}
