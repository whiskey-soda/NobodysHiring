using UnityEngine;

public class DayEnd : MonoBehaviour
{
    [SerializeField] float energyBoostThresholdPercent = .3f;
    [SerializeField] float motivationBoostAmount = 8;

    SleepManager sleep;
    PlayerStats playerStats;

    public static DayEnd Instance;
    private void Awake()
    {
        // singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sleep = SleepManager.Instance;
        playerStats = PlayerStats.Instance;
    }

    /// <summary>
    /// ends the day by sleeping for a set amount of hours
    /// </summary>
    /// <param name="sleepHours"></param>
    public void EndDay(float sleepHours)
    {
        bool significantEnergyRemaining = playerStats.energy > energyBoostThresholdPercent * playerStats.energyMax;

        sleep.Sleep(sleepHours);

        // give motivation boost based on if energy was left over on previous day
        // simulates pacing yourself and maintaining your excitement for work
        if (significantEnergyRemaining)
        {
            playerStats.ChangeMotivation(motivationBoostAmount);
        }
    }


}
