using UnityEngine;

public enum LifeFactor
{
    UnpaidBills, Cleanliness, HousingQuality,
    SavingsAmount, WorkstationQuality
}

public class LifeFactors : MonoBehaviour
{
    public float[] factorValues = { 0, 0, 0, 0, 0 };

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

    public void RaiseFactorCoefficient(LifeFactor factor, float value)
    {
        factorValues[(int)factor] += value;

        // cap at 1
        if (factorValues[(int)factor] > 1)
        {
            factorValues[(int)factor] = 1;
        }
    }

}
