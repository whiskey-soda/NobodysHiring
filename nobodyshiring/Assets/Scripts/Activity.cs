using UnityEngine;

public class Activity : MonoBehaviour
{
    public string activityName;
    public float duration;
    public string description;

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

    public virtual void DoActivity()
    {
        time.PassTime(duration);
        playerStats.ChangeMotivation(-motivationCost);
        playerStats.ChangeEnergy(-energyCost);
    }

}
