using UnityEngine;

public class WorkOnProject : Activity
{
    [Space]
    [SerializeField] float progressMultiplier = 1; // used for working at different paces
    [Space]
    [Tooltip("skill improvement per hour when skill is less than required level")]
    [SerializeField] float lowSkillImprovementRate = .015f;
    [Tooltip("skill improvement per hour when skill is equal to required level")]
    [SerializeField] float equalSkillImprovementRate = .01f;
    [Tooltip("skill improvement per hour when skill is more than required level")]
    [SerializeField] float highSkillImprovementRate = .005f;
    public void DoActivity(float duration, Project project)
    {
        base.DoActivity(duration);

        if (project is Job)
        {
            ((Job)project).Work(progressMultiplier * duration);
            TrainSkills((Job)project, duration);
        }
        else if (project is Gig)
        {
            ((Gig)project).currentStage.Work(progressMultiplier * duration);
            TrainSkills( ((Gig)project).currentStage, duration);
        }
    }

    void TrainSkills(Task task, float duration)
    {
        PlayerSkills playerSkills = PlayerSkills.Instance;

        for (int i = 0; i < task.recommededSkillLevels.Length; i++)
        {
            if (task.recommededSkillLevels[i] != 0)
            {
                float skillImprovementRate = 0;

                // skill lower than requirements, learn more but work slower
                if ((int)(task.recommededSkillLevels[i]) > (int)(playerSkills.skills[i]))
                {
                    skillImprovementRate = lowSkillImprovementRate;
                    Debug.Log($"{((Skill)i).ToString()} lower than requirement! learning {lowSkillImprovementRate} per hour");
                }
                // skill equal to requirements. no bonus learning
                else if ((int)(task.recommededSkillLevels[i]) == (int)(playerSkills.skills[i]))
                {
                    skillImprovementRate = equalSkillImprovementRate;
                    Debug.Log($"{((Skill)i).ToString()} equal to requirement! learning {equalSkillImprovementRate} per hour");
                }
                // skill higher than requirements. easy/fast work, but you don't learn much
                else if ((int)(task.recommededSkillLevels[i]) < (int)(playerSkills.skills[i]))
                {
                    skillImprovementRate = highSkillImprovementRate;
                    Debug.Log($"{((Skill)i).ToString()} higher than requirement! learning {highSkillImprovementRate} per hour");
                }

                PlayerSkills.Instance.ChangeSkill((Skill)i, skillImprovementRate * duration);
            }
        }
    }

}
