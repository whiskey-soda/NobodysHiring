using UnityEngine;

public class SleepManager : MonoBehaviour
{
    TimeTracking time;
    PlayerStats stats;

    [SerializeField] float wellRestedHours = 9;
    [SerializeField] float tiredHours = 6;
    [Space]
    [SerializeField] float restedEnergy = 100;
    [SerializeField] float wellRestedEnergy = 120;
    [SerializeField] float tiredEnergy = 70;

    private void Start()
    {
        time = TimeTracking.Instance;
        stats = PlayerStats.Instance;
    }


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

}
