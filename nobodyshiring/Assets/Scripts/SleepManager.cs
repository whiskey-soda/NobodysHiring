using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent PassedOut;
    public UnityEvent sleep;

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

        if (duration >= wellRestedHours) // well rested gives bonus energy
        {
            newEnergyLevel = wellRestedEnergy;
        }
        else if (duration < tiredHours) // duration in tired range gives less energy depending as duration approaches 0
        {
            // tiredenergy is maximum amount recovered when sleeping less than the "tired" threshold
            // recover less than tiredEnergy based on how much less you slept than the tired threshold, to a min of 0
            newEnergyLevel = (duration / tiredHours) * tiredEnergy;
        }

        stats.SetEnergy(newEnergyLevel);

        sleep.Invoke();

    }

    /// <summary>
    /// passes time and sets players energy to a "tired" level
    /// </summary>
    public void PassOut()
    {
        time.PassTime(passOutDuration);
        stats.SetEnergy(tiredEnergy);

        PassedOut.Invoke();
        sleep.Invoke();

        Debug.Log($"passed out for {passOutDuration} hours!");
    }

}
