using UnityEngine;

public enum LifeFactor
{
    UnpaidBills, Cleanliness, HousingQuality,
    SavingsAmount, WorkstationQuality
}

public class LifeFactors : MonoBehaviour
{
    public float[] factorValues = { 0, 0, 0, 0, 0 }; // 0 is bad, 1 is good

    public static LifeFactors Instance;

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

    /// <summary>
    /// adds a value to a given factor's value
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="value"></param>
    public void ChangeFactorValue(LifeFactor factor, float value)
    {
        factorValues[(int)factor] += value;
        ClampFactorValue(factor);
    }

    /// <summary>
    /// sets a factor's value
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="value"></param>
    public void SetFactorValue(LifeFactor factor, float value)
    {
        factorValues[(int)factor] = value;
        ClampFactorValue(factor);
    }

    /// <summary>
    /// clamps a factor value between 0 and 1
    /// </summary>
    /// <param name="factor"></param>
    private void ClampFactorValue(LifeFactor factor)
    {
        // clamp from 0-1
        if (factorValues[(int)factor] > 1)
        {
            factorValues[(int)factor] = 1;
        }
        else if (factorValues[(int)factor] < 0)
        {
            factorValues[(int)factor] = 0;
        }
    }


}
