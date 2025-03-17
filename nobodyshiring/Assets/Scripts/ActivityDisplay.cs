using TMPro;
using UnityEngine;

public class ActivityDisplay : MonoBehaviour
{
    [SerializeField] Activity activity;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI durationText;
    [SerializeField] TextMeshProUGUI descriptionText;


    // Update is called once per frame
    void Update()
    {
        nameText.text = activity.activityName;
        durationText.text = $"Duration: {activity.duration} hours";
        descriptionText.text = $"Description: {activity.description}";
    }
}
