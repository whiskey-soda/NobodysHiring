using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillTrainingUIHelper : ActivityUIHelper
{
    [SerializeField] TMP_Dropdown skillDropdown;
    [SerializeField] float improvementPerHour = .02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDropdown();
    }

    void UpdateDropdown()
    {
        skillDropdown.ClearOptions();
        skillDropdown.AddOptions(Enum.GetNames(typeof(Skill)).ToList<String>());
    }

    public override void DoActivity()
    {
        activity.skillChanges.Clear();
        activity.skillChanges.Add(new SkillChange((Skill)skillDropdown.value, improvementPerHour));
        activity.DoActivity(desiredDuration);
    }
}
