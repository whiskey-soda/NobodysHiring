using UnityEngine;

public class SkillBarMask : ProgressBarMask
{
    [Space]
    [SerializeField] Skill skill;
    [SerializeField] float skillMax = 4;
    PlayerSkills skillsScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillsScript = PlayerSkills.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaskRect(skillsScript.skills[(int)skill], skillMax);
    }
}
