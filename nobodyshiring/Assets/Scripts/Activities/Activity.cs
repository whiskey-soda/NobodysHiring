using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SkillChange
{
    public Skill skill;
    public float value;

    public SkillChange(Skill skill, float value)
    {
        this.skill = skill;
        this.value = value;
    }
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

    [SerializeField] bool oncePerDay = false;
    bool isAvailable = true;

    TimeTracking time;
    PlayerStats playerStats;
    LifeFactors lifeFactors;

    [Space]
    [SerializeField] float motivationGain;
    [SerializeField] float energyGain;

    [Space]
    [SerializeField] List<LifeFactorChange> lifeFactorChanges;
    [SerializeField] public List<SkillChange> skillChanges;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        time = TimeTracking.Instance;
        playerStats = PlayerStats.Instance;
        lifeFactors = LifeFactors.Instance;

        // for resetting availability at the end of the day
        if (oncePerDay) { time.dayEnd.AddListener(ResetLimitedAvailability); }
    }

    public virtual void DoActivity(float duration)
    {
        // do nothing if activity is not available
        if (!isAvailable) { return; }

        duration = ClampDuration(duration);

        // adjusts the duration based on how much time was spent before the player passed out.
        // only counts time spent working BEFORE passing out
        duration *= playerStats.ChangeEnergy(-energyCost * duration);

        time.PassTime(duration);
        playerStats.ChangeMotivation(-motivationCost * duration);

        playerStats.ChangeMotivation(motivationGain * duration);
        playerStats.ChangeEnergy(energyGain * duration);

        // apply life factor changes
        foreach (LifeFactorChange change in lifeFactorChanges)
        {
            lifeFactors.ChangeFactorValue(change.lifeFactor, change.value * duration);
        }

        // apply skill changes
        foreach (SkillChange change in skillChanges)
        {
            PlayerSkills.Instance.ChangeSkill(change.skill, change.value * duration);
        }


        // make activity unavailable if it is limited to once per day
        if (oncePerDay) { isAvailable = false; }

    }

    /// <summary>
    /// clamps the duration of the activity to the activity's max/min duration
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    protected float ClampDuration(float duration)
    {
        // clamp duration to min/max values
        if (duration < minDuration) { duration = minDuration; }
        else if (maxDuration != 0 && duration > maxDuration) { duration = maxDuration; }

        return duration;
    }

    /// <summary>
    /// if the activity is limited to once per day, it is made available. this method is used at the end of a day.
    /// </summary>
    void ResetLimitedAvailability()
    {
        if (oncePerDay && !isAvailable)
        {
            isAvailable = true;
        }
    }
}
