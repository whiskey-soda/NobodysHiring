using Unity.Burst.Intrinsics;
using UnityEngine;

[System.Serializable]
public class Task : Project
{
    public string taskName;

    public float maxProgress;
    public float currentProgress;
    public bool complete { get; protected set; } = false;

    public float[] recommededSkillLevels { get; private set; } = new float[10];

    public float pay { get; private set; }

    // multipliers that apply at different levels of skill difference
    // between player skills and recommended skills
    float lowSkillDiffMult = .3f;
    float highSkillDiffMult = .7f;
    float extremeSkillDiffMult = 1;

    float highMotivationMult = .35f;
    float highEnergyMult = .15f;

    float lowMotivationPenaltyCoefficient = .5f;
    float lowEnergyPenaltyCoefficient = .5f;


    public Task(TaskData data)
    {
        Init(data);
    }

    /// <summary>
    /// configures task with values from a scriptable object
    /// </summary>
    /// <param name="data"></param>
    protected virtual void Init(TaskData data)
    {
        float[] _recommendedSkillLevels = { data.recommendedCoding, data.recommendedNetworking,
            data.recommendedMarketing, data.recommendedGameDesign, data.recommendedUnity,
            data.recommendedUnrealEngine, data.recommendedGodot, data.recommendedGameMaker,
            data.recommendedAudio, data.recommendedArt };

        recommededSkillLevels = _recommendedSkillLevels;

        maxProgress = data.maxProgress;
        pay = data.pay;
    }


    /// <summary>
    /// adds progress with a multiplier calculated from various stats
    /// </summary>
    /// <param name="hours"></param>
    public void Work(float hours)
    {
        float progressMult = CalculateProgressMult();
        float progressCompleted = hours * progressMult;

        // overflow prevention. stops task early (when it is completed) if progress overflows
        // takes the overflow progress, divides it by the multiplier to get the hours, and rounds up to get duration
        if (currentProgress + progressCompleted > maxProgress)
        {
            hours = ((currentProgress + progressCompleted) - maxProgress) / progressMult;
            hours = Mathf.Ceil(hours);

            currentProgress = maxProgress; //set progress to max
        }
        else
        {
            // add progress to current progress
            currentProgress += progressCompleted;
        }

        if (currentProgress >= maxProgress)
        {
            Complete();
        }
    }

    /// <summary>
    /// marks this task as complete
    /// </summary>
    protected virtual void Complete()
    {
        complete = true;
        Money.Instance.AddMoney(pay);
        WorkManager.Instance.workListUpdated.Invoke();
    }

    /// <summary>
    /// calculates the total multiplier applied to progress,
    /// based on skill levels and stats.
    /// </summary>
    /// <returns></returns>
    private float CalculateProgressMult()
    {
        float progressMult = 1;

        // additive bonuses to progress are awarded first
        progressMult += CalculateMultFromSkills();
        progressMult += CalculateMultBonusFromStats();

        // multiplicative penalty is applied last
        progressMult *= CalculateMultPenaltyCoefficientFromStats();

        // capped at a minimum of 0
        if (progressMult < 0) { progressMult = 0; }

        return progressMult;
    }

    /// <summary>
    /// calculates the multiplier applies to progress each hour,
    /// based on the players skill levels and the tasks skill levels
    /// </summary>
    /// <returns></returns>
    private float CalculateMultFromSkills()
    {
        float multFromSkills = 0;

        PlayerSkills playerSkills = PlayerSkills.Instance;

        for (int i = 0; i < playerSkills.skills.Length; i++)
        {
            // if skill is relevant to the task (as in, not 0 recommended level)...
            if (recommededSkillLevels[i] != 0)
            {
                // give a bonus multiplier to progress if skill level is above or below rec level

                float skillDiff = playerSkills.skills[i] - recommededSkillLevels[i];
                float progressMultAwarded = 0;

                if (Mathf.Abs(skillDiff) > 2)
                {
                    progressMultAwarded = extremeSkillDiffMult;
                }
                else if (Mathf.Abs(skillDiff) > 1)
                {
                    progressMultAwarded = highSkillDiffMult;
                }
                else if (Mathf.Abs(skillDiff) > 0)
                {
                    progressMultAwarded = lowSkillDiffMult;
                }

                multFromSkills += Mathf.Sign(skillDiff) * progressMultAwarded;

            }
        }

        return multFromSkills;
    }

    /// <summary>
    /// calculates the progress multiplier bonus awarded from high motivation and/or high energy
    /// </summary>
    /// <returns></returns>
    float CalculateMultBonusFromStats()
    {
        PlayerStats playerStats = PlayerStats.Instance;

        float multFromStats = 0;

        if (playerStats.motivation > playerStats.highMotivationThreshold)
        {
            multFromStats += highMotivationMult;
        }

        if (playerStats.energy > playerStats.highEnergyThreshold)
        {
            multFromStats += highEnergyMult;
        }

        return multFromStats;
    }

    /// <summary>
    /// calculates the penalty coefficient based on low energy and/or motivation
    /// </summary>
    /// <returns></returns>
    float CalculateMultPenaltyCoefficientFromStats()
    {
        PlayerStats playerStats = PlayerStats.Instance;

        // motivation and energy each make up half of the progress penalty coefficient.
        // low stats will lower the coefficient, dividing the amount of progress gained.

        float penaltyCoefficient = 1;

        // motivation
        if (playerStats.motivation < playerStats.lowMotivationThreshold)
        {
            float interpValue = (playerStats.lowMotivationThreshold - playerStats.motivation) / playerStats.lowMotivationThreshold;
             penaltyCoefficient -= Mathf.Lerp(0, lowMotivationPenaltyCoefficient, interpValue);
        }

        //energy
        if (playerStats.energy < playerStats.lowEnergyThreshold)
        {
            float interpValue = (playerStats.lowEnergyThreshold - playerStats.energy) / playerStats.lowEnergyThreshold;
            penaltyCoefficient -= Mathf.Lerp(0, lowEnergyPenaltyCoefficient, interpValue);
        }

        return penaltyCoefficient;
    }

}
