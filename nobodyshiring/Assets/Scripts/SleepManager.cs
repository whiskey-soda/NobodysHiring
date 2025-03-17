using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance;

    TimeTracking time;
    PlayerStats stats;

    [SerializeField] float wellRestedHours = 9;
    [SerializeField] float tiredHours = 6;
    [Space]
    [SerializeField] float restedEnergy = 100;
    [SerializeField] float wellRestedEnergy = 120;
    [SerializeField] float tiredEnergy = 70;
    [Space]
    [SerializeField] float passOutDuration = 11;

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

    private void Start()
    {
        time = TimeTracking.Instance;
        stats = PlayerStats.Instance;
    }

    /// <summary>
    /// passes time and restores energy based on how long the player slept
    /// </summary>
    /// <param name="duration"></param>
    public void Sleep(float duration)
    {
        time.PassTime(duration);

        float newEnergyLevel = restedEnergy;

        if (duration >= wellRestedHours)
        {
            newEnergyLevel = wellRestedEnergy;
        }
        else if (duration < tiredHours)
        {
            newEnergyLevel = tiredEnergy;
        }

        stats.SetEnergy(newEnergyLevel);

    }

    /// <summary>
    /// passes time and sets players energy to a "tired" level
    /// </summary>
    public void PassOut()
    {
        time.PassTime(passOutDuration);
        stats.SetEnergy(tiredEnergy);
    }

}
