using TMPro;
using UnityEngine;

public class JobOfferUIHelper : WorkOfferUIHelper
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI recommendedSkillsText;

    public override void Configure(WorkOffer workOffer)
    {
        PopulateJobInfo((JobOffer)workOffer);
    }

    void PopulateJobInfo(JobOffer jobOffer)
    {
        nameText.text = jobOffer.job.taskName + " ($" + jobOffer.job.pay + "/month)";

        // clear recommended skills text, then populate with recommended skill levels
        recommendedSkillsText.text = "";
        PopulateSkillRecs(jobOffer.job);
    }

    private void PopulateSkillRecs(Task task)
    {
        for (int i = 0; i < task.recommededSkillLevels.Length; i++)
        {
            // if skill level is not 0, display its recommendation
            if (task.recommededSkillLevels[i] != 0)
            {
                recommendedSkillsText.text += $"{(Skill)i}: {task.recommededSkillLevels[i]}\n";
            }

        }
    }

}
