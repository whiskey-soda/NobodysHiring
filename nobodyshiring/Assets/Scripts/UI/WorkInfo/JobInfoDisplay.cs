using TMPro;
using UnityEngine;

public class JobInfoDisplay : WorkInfoDisplay
{
    Job job;

    [SerializeField] TextMeshProUGUI jobName;
    [SerializeField] TextMeshProUGUI pay;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] TextMeshProUGUI estimatedHours;
    [SerializeField] TextMeshProUGUI skillReqs;

    public override void Configure(Job job)
    {
        this.job = job;
    }

    private void Update()
    {
        if (job == null) { return; }

        jobName.text = $"{job.taskName}";
        pay.text = $"Monthly Pay: {job.pay}";
        progress.text = $"Progress: {(job.currentProgress / job.maxProgress) * 100}%";
        estimatedHours.text = $"Estimated Monthly Hours = {job.maxProgress}";

        skillReqs.text = "";
        for (int i = 0; i < job.recommededSkillLevels.Length; i++)
        {
            if (job.recommededSkillLevels[i] != 0)
            {
                skillReqs.text += $"{(Skill)i}: {job.recommededSkillLevels[i]}\n";
            }
        }
    }

}
