using Unity.Burst.Intrinsics;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string taskName;

    public float progressMax;
    public float currentProgress;
    public bool complete { get; protected set; } = false;
    [Space]

    // used for config and inspector editing
    [SerializeField] float recommendedCoding;
    [SerializeField] float recommendedNetworking;
    [SerializeField] float recommendedMarketing;
    [SerializeField] float recommendedGameDesign;
    [SerializeField] float recommendedUnity;
    [SerializeField] float recommendedUnrealEngine;
    [SerializeField] float recommendedGodot;
    [SerializeField] float recommendedGameMaker;
    [SerializeField] float recommendedAudio;
    [SerializeField] float recommendedArt;

    // used for actual processing
    public float[] recommededSkillLevels { get; private set; } = new float[10];


    float lowSkillDiffMult = .3f;
    float highSkillDiffMult = .7f;
    float extremeSkillDiffMult = 1;

    float highMotivationMult = .35f;
    float highEnergyMult = .15f;

    float lowMotivationPenaltyCoefficient = .5f;
    float lowEnergyPenaltyCoefficient = .5f;


    PlayerStats playerStats;
    PlayerSkills playerSkills;
    TimeTracking time;


    protected virtual void Start()
    {
        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;
        time = TimeTracking.Instance;
    }

    private void Awake()
    {
        SetSkillRecs();
    }

    /// <summary>
    /// sets the recommended skill levels array based on the config floats.
    /// makes it easier to edit values in the editor.
    /// </summary>
    void SetSkillRecs()
    {
        float[] _recommendedSkillLevels = { recommendedCoding, recommendedNetworking,
            recommendedMarketing, recommendedGameDesign, recommendedUnity,
            recommendedUnrealEngine, recommendedGodot, recommendedGameMaker,
            recommendedAudio, recommendedArt };

        recommededSkillLevels = _recommendedSkillLevels;
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
        if (currentProgress + progressCompleted > progressMax)
        {
            hours = ((currentProgress + progressCompleted) - progressMax) / progressMult;
            hours = Mathf.Ceil(hours);

            currentProgress = progressMax; //set progress to max
        }
        else
        {
            // add progress to current progress
            currentProgress += progressCompleted;
        }

        if (currentProgress >= progressMax)
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
