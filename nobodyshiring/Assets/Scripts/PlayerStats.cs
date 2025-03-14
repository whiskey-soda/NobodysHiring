using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    float motivationMax = 100;
    public float motivation { get; private set; } = 100;
    public float energy { get; private set; } = 100;


    public static PlayerStats Instance;

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

        motivation = motivationMax;
    }

    public void ChangeMotivation(float value)
    {
        motivation += value;
    }

    public void ChangeEnergy(float value)
    {
        energy += value;
    }

}
