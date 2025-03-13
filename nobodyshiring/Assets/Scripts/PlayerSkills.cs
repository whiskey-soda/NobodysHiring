using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public float[] skills = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


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
