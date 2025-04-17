using TMPro;
using UnityEngine;

public class GigOfferUIHelper : WorkOfferUIHelper
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI stagesInfoText;

    public override void Configure(WorkOffer workOffer)
    {
        offer = workOffer;
        PopulateGigInfo((GigOffer)workOffer);
    }

    void PopulateGigInfo(GigOffer gigOffer)
    {
        nameText.text = gigOffer.gig.gigName;

        foreach (Task stage in gigOffer.gig.stages)
        {
            stagesInfoText.text += $"{stage.taskName} (${stage.pay}\n";
            for (int i = 0; i < stage.recommededSkillLevels.Length; i++)
            {
                // if skill level is not 0, display its recommendation
                if (stage.recommededSkillLevels[i] != 0)
                {
                    stagesInfoText.text += $"{(Skill)i}: {stage.recommededSkillLevels[i]}\n";
                }
            }
            stagesInfoText.text += "\n"; 
        }
    }

    private void PopulateSkillRecs(Task task)
    {
        for (int i = 0; i < task.recommededSkillLevels.Length; i++)
        {
            // if skill level is not 0, display its recommendation
            if (task.recommededSkillLevels[i] != 0)
            {
                stagesInfoText.text += $"{(Skill)i}: {task.recommededSkillLevels[i]}\n";
            }

        }
    }
}
