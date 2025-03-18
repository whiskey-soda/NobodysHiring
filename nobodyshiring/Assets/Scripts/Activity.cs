using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
struct SkillChange
{
    public Skill skill;
    public float value;
}

[System.Serializable]
struct LifeFactorChange
{
    public LifeFactor lifeFactor;
    public float value;
}

public class Activity : MonoBehaviour
{
    public string activityName;
    public string description;

    // EVERY CHANGE IS PER HOUR
    [SerializeField] float motivationCost;
    public float energyCost;

    [SerializeField]
    public float minDuration = 0;
    public float maxDuration = 0; // 0 means uncapped

    TimeTracking time;
    PlayerStats playerStats;
    LifeFactors lifeFactors;

    [Space]
    [SerializeField] float motivationGain;
    [SerializeField] float energyGain;

    [Space]
    [SerializeField] List<LifeFactorChange> lifeFactorChanges;
    [SerializeField] List<SkillChange> skillChanges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = TimeTracking.Instance;
        playerStats = PlayerStats.Instance;
        lifeFactors = LifeFactors.Instance;
    }

    public virtual void DoActivity(float duration)
    {
        // clamp duration to min/max values
        if (duration < minDuration) { duration = minDuration; }
        else if (maxDuration != 0 && duration > maxDuration) { duration = maxDuration; }

        time.PassTime(duration);
        playerStats.ChangeMotivation(-motivationCost * duration);
        playerStats.ChangeEnergy(-energyCost * duration);

        playerStats.ChangeMotivation(motivationGain * duration);
        playerStats.ChangeEnergy(energyGain * duration);

        foreach (LifeFactorChange change in lifeFactorChanges)
        {
            lifeFactors.ChangeFactorCoefficient(change.lifeFactor, change.value * duration);
        }

        foreach (SkillChange change in skillChanges)
        {
            PlayerSkills.Instance.ChangeSkill(change.skill, change.value * duration);
        }
    }

}
