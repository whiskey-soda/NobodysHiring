using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockOutUIHelper : MonoBehaviour
{
    [SerializeField] TMP_InputField sleepHrsInput;
    [SerializeField] GameObject dayActivities;
    [SerializeField] GameObject clockedOutActivities;
    [SerializeField] Button sleepButton;

    DayEnd dayEnd;

    private void Start()
    {
        dayEnd = DayEnd.Instance;

        // so if you pass out while clocked out, you get clocked in when you wake up
        SleepManager.Instance.PassedOut.AddListener(ShowClockedInActivities);

        ShowClockedInActivities();
    }

    public void EndDay()
    {
        dayEnd.EndDay(float.Parse(sleepHrsInput.text));
        ShowClockedInActivities();
    }

    public void ShowClockedOutActivities()
    {
        dayActivities.SetActive(false);
        clockedOutActivities.SetActive(true);
        sleepButton.interactable = true;
    }

    void ShowClockedInActivities()
    {
        dayActivities.SetActive(true);
        clockedOutActivities.SetActive(false);
        sleepButton.interactable = false;
    }
}
