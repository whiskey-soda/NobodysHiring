using UnityEngine;

public class Activity : MonoBehaviour
{
    [SerializeField] float duration;

    [SerializeField] float motivationCost;
    [SerializeField] float energyCost;

    TimeTracking time;
    PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = TimeTracking.Instance;
        playerStats = PlayerStats.Instance;
    }

    [ContextMenu("DoActivity")]
    protected virtual void DoActivity()
    {
        time.PassTime(duration);
        playerStats.ChangeMotivation(-motivationCost);
        playerStats.ChangeEnergy(-energyCost);
    }

}
