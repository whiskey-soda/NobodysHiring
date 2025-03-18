using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SkillsDisplay : MonoBehaviour
{
    PlayerSkills playerSkills;

    TextMeshProUGUI statsText;

    private void Awake()
    {
        statsText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSkills = PlayerSkills.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        statsText.text = $"skills\n";
        for (int i = 0; i < playerSkills.skills.Length; i++)
        {
            statsText.text += $"{(Skill)i}: {System.Math.Round(playerSkills.skills[i], 2)}\n";
        }
    }
}
