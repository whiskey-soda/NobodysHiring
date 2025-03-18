using UnityEngine;

public enum Skill
{
    Coding, Networking, Marketing,
    GameDesign, Unity, UnrealEngine,
    Godot, GameMaker,
    Audio, Art
}

public class PlayerSkills : MonoBehaviour
{
    public float[] skills = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    float maxSkillLevel = 4;

    public static PlayerSkills Instance;

    private void Awake()
    {
        // singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void ChangeSkill(Skill skill, float value)
    {
        skills[(int)skill] += value;

        // cap skill level
        if (skills[(int)skill] > maxSkillLevel)
        {
            skills[(int)skill] = maxSkillLevel;
        }
    }

}
