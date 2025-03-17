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
    public float duration;
    public string description;

    [SerializeField] float motivationCost;
    [SerializeField] float energyCost;

    TimeTracking time;
    PlayerStats playerStats;
    LifeFactors lifeFactors;

    [Space]
    [SerializeField] float motivationGain;

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

    public virtual void DoActivity()
    {
        time.PassTime(duration);
        playerStats.ChangeMotivation(-motivationCost);
        playerStats.ChangeEnergy(-energyCost);

        playerStats.ChangeMotivation(motivationGain);

        foreach (LifeFactorChange change in lifeFactorChanges)
        {
            lifeFactors.ChangeFactorCoefficient(change.lifeFactor, change.value);
        }

        foreach (SkillChange change in skillChanges)
        {
            PlayerSkills.Instance.ChangeSkill(change.skill, change.value);
        }
    }

}
