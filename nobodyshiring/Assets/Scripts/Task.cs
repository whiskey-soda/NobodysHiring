using Unity.Burst.Intrinsics;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] float progressMax;
    [SerializeField] float currentProgress;
    [Space]

    [SerializeField] float motivationCostPerHour = 10;
    [SerializeField] float energyCostPerHour = 25;
    [Space]

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

    float[] recommededSkillLevels = new float[10];

    PlayerStats playerStats;
    PlayerSkills playerSkills;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;
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

    void Work(float hours)
    {
        float progressCoefficient = CalculateProgressCoefficient();

        currentProgress += hours * progressCoefficient;

        playerStats.ChangeMotivation(-motivationCostPerHour * hours);
        playerStats.ChangeEnergy(-energyCostPerHour * hours);
    }

    /// <summary>
    /// calculates the multiplier applies to progress each hour,
    /// based on the players skill levels and the tasks skill levels
    /// </summary>
    /// <returns></returns>
    private float CalculateProgressCoefficient()
    {
        float progressCoefficient = 1;

        for (int i = 0; i < playerSkills.skills.Length; i++)
        {
            // if skill is relevant to the task (as in, not 0 recommended level)...
            if (recommededSkillLevels[i] != 0)
            {
                // give a bonus multiplier to progress if skill level is above or below rec level

                float skillDiff = playerSkills.skills[i] - recommededSkillLevels[i];
                float progressMultAwarded = 0;

                if (Mathf.Abs(skillDiff) >= 3)
                {
                    progressMultAwarded = 1;
                }
                else if (Mathf.Abs(skillDiff) >= 2)
                {
                    progressMultAwarded = .75f;
                }
                else if (Mathf.Abs(skillDiff) >= 1)
                {
                    progressMultAwarded = .5f;
                }

                progressCoefficient += Mathf.Sign(skillDiff) * progressMultAwarded;

            }
        }

        return progressCoefficient;
    }
}
