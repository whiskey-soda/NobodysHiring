using UnityEngine;

public class MotivationDrain : MonoBehaviour
{
    float[] factorWeights = { .3f, .25f, .2f, .15f, .1f };

    PlayerStats playerStats;
    LifeFactors lifeFactors;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        lifeFactors = LifeFactors.Instance;
    }

    void ApplyMotivationDrain(float drainValue)
    {
        float drainAmount = drainValue;
        for (int i = 0; i < lifeFactors.factorValues.Length; i++)
        {
            drainAmount -= lifeFactors.factorValues[i] * factorWeights[i] * drainValue;
        }

        playerStats.motivation -= drainAmount;
    }

}
