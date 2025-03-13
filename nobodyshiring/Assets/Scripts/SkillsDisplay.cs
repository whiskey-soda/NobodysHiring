using TMPro;
using UnityEngine;

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
        statsText.text = $"skills\n" +
            $"software engineering: {playerSkills.softwareEng}\n" +
            $"networking: {playerSkills.networking}\n" +
            $"marketing: {playerSkills.marketing}\n" +
            $"game design: {playerSkills.gameDesign}\n" +
            $"unity: {playerSkills.unity}\n" +
            $"unreal engine: {playerSkills.unrealEngine}\n" +
            $"godot: {playerSkills.godot}\n" +
            $"gamemaker: {playerSkills.gameMaker}\n" +
            $"audio: {playerSkills.audioSkill}\n" +
            $"art: {playerSkills.art}";
    }
}
