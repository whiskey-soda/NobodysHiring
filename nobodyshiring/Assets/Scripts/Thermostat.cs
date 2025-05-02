using UnityEngine;

public class Thermostat : MonoBehaviour
{
    public bool On = false;
    public float motivationCostMult = 1.2f;
    public float energyCostMult = 1.2f;

    public static Thermostat Instance;
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

    void TurnOn()
    {
        On = true;
    }

    void TurnOff()
    {
        On = false;
    }

    public void Toggle()
    {
        On = !On;
    }
}
