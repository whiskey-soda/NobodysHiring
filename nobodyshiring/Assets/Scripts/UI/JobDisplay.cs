using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobDisplay : MonoBehaviour
{
    [SerializeField] Job job;
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
        nameText.text = job.taskName;
        progressText.text = $"Progress: {System.Math.Round(job.currentProgress, 2)} / {job.maxProgress}";
        payText.text = $"Pay: ${job.pay}";

        minDurationText.text = "0";
        durationText.text = "Duration: " + System.Math.Round(durationSlider.value, 2).ToString();

        /*
        // set max duration on the slider to maximum hours before passing out
        float hoursUntilNoEnergy = playerStats.energy / job.energyCostPerHour;

        durationSlider.maxValue = hoursUntilNoEnergy;
        maxDurationText.text = System.Math.Round(hoursUntilNoEnergy, 2).ToString();
        */

        // button only interactable if job is incomplete
        if (!job.complete)
        {
            workButton.interactable = true;
        }
        else
        {
            workButton.interactable = false;
        }
    }
}
