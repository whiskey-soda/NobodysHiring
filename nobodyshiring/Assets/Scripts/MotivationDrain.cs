using UnityEngine;

public class MotivationDrain : MonoBehaviour
{
    [SerializeField] float unpaidBillsWeight = .3f;
    [SerializeField] float cleanlinessWeight = .25f;
    [SerializeField] float housingQualityWeight = .2f;
    [SerializeField] float savingsAmountWeight = .15f;
    [SerializeField] float workstationQualityWeight = .1f;

    float[] factorWeights = new float[5];

    PlayerStats playerStats;
    LifeFactors lifeFactors;

    private void Awake()
    {
        SetWeights();
    }

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        lifeFactors = LifeFactors.Instance;
    }

    /// <summary>
    /// sets the life factor weights array based on the config floats.
    /// makes it easier to edit values in the editor.
    /// </summary>
    void SetWeights()
    {
        float[] _factorWeights = { unpaidBillsWeight, cleanlinessWeight,
        housingQualityWeight, savingsAmountWeight, workstationQualityWeight };

        factorWeights = _factorWeights;
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
