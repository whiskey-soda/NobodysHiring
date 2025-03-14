using UnityEngine;

public enum LifeFactor
{
    UnpaidBills, Cleanliness, HousingQuality,
    SavingsAmount, WorkstationQuality
}

public class LifeFactors : MonoBehaviour
{
    public float[] factorCoefficients = { 0, 0, 0, 0, 0 };
    float[] factorWeights = { .3f, .25f, .2f, .15f, .1f };

    float motivationDrainMax = 50;

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
        factorCoefficients[(int)factor] += value;
    }

}
