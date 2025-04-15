using TMPro;
using UnityEngine;

public class GigInfoDisplay : WorkInfoDisplay
{
    Gig gig;

    [SerializeField] TextMeshProUGUI gigProgress;
    [SerializeField] TextMeshProUGUI stageProgress;
    [SerializeField] TextMeshProUGUI estimatedHours;
    [SerializeField] TextMeshProUGUI stageInfos;

    public override void Configure(Gig gig)
    {
        this.gig = gig;
    }

    private void Update()
    {
        if (gig == null) { return; }

        workName.text = $"{gig.gigName}";
        gigProgress.text = $"Stage: {gig.stages.IndexOf(gig.currentStage) + 1} / {gig.stages.Count}";
        stageProgress.text = $"Progress: {(gig.currentStage.currentProgress / gig.currentStage.maxProgress) * 100}%";
        estimatedHours.text = $"Estimated Hours = {gig.currentStage.maxProgress}";

        stageInfos.text = "";

        foreach (GigStage stage in gig.stages)
        {
            stageInfos.text += $"{gig.currentStage.taskName}\n" +
                $"Pay: ${gig.currentStage.pay}\n" +
                $"Skill Requirements:\n";

            for (int i = 0; i < gig.currentStage.recommededSkillLevels.Length; i++)
            {
                if (gig.currentStage.recommededSkillLevels[i] != 0)
                {
                    stageInfos.text += $"{(Skill)i}: {gig.currentStage.recommededSkillLevels[i]}\n";
                }
            }
            stageInfos.text += "\n";
        }
    }
}
