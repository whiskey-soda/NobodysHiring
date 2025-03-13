using UnityEngine;

public class PlayerSkills : MonoBehaviour
{

    public float softwareEng = 0;
    public float networking = 0;
    public float marketing = 0;

    public float gameDesign = 0;
    public float unity = 0;
    public float unrealEngine = 0;
    public float godot = 0;
    public float gameMaker = 0;
    public float audioSkill = 0;
    public float art = 0;


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

}
