using UnityEngine;

public class DayEnd : MonoBehaviour
{
    [SerializeField] float energyBoostThresholdPercent = .3f;
    [SerializeField] float energyBoostAmount = 20;
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

        // give energy boost based on if energy was left over on previous day
        if (significantEnergyRemaining)
        {
            playerStats.ChangeEnergy(energyBoostAmount);
            playerStats.ChangeMotivation(motivationBoostAmount);
        }
    }


}
