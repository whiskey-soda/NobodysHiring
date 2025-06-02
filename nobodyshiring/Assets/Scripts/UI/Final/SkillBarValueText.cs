using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]

public class SkillBarValueText : MonoBehaviour
{
    TextMeshProUGUI valueText;
    PlayerSkills skillsScript;
    [SerializeField] Skill skill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        valueText = GetComponent<TextMeshProUGUI>();
        skillsScript = PlayerSkills.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        valueText.text = System.Math.Round(skillsScript.skills[(int)skill], 2).ToString();
    }
}
