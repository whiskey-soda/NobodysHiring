using System;
using UnityEngine;
using System.Linq;

public class MotivationDrain : MonoBehaviour
{
    [SerializeField] float unpaidBillsWeight = .5f;
    [SerializeField] float cleanlinessWeight = .25f;
    [SerializeField] float groceryQualityWeight = .15f;
    [SerializeField] float housingQualityWeight = .2f;
    [SerializeField] float savingsAmountWeight = .15f;
    [SerializeField] float workstationQualityWeight = .1f;

    float[] factorWeights;

    [SerializeField] float dailyDrainMax = 10;
    float hourlyDrainMax;

    PlayerStats playerStats;
    LifeFactors lifeFactors;

    public static MotivationDrain Instance;

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

        // initialize weights array
        factorWeights = new float[Enum.GetNames(typeof(LifeFactor)).Count()];
        SetWeights();

        hourlyDrainMax = dailyDrainMax / 24;

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
        float[] _factorWeights = { unpaidBillsWeight, cleanlinessWeight, groceryQualityWeight,
        housingQualityWeight, savingsAmountWeight, workstationQualityWeight };

        factorWeights = _factorWeights;
    }

    /// <summary>
    /// reduces motivation for every hour passed. drain value is reduced according to life factors
    /// </summary>
    /// <param name="hours"></param>
    public void ApplyMotivationDrain(float hours)
    {
        float drainAmount = hours * hourlyDrainMax;

        float weightsTotal = 0;
        foreach (float weight in factorWeights)
        {
            weightsTotal += weight;
        }

        // subtracts different amounts from the drain max value depending on life factors
        for (int i = 0; i < lifeFactors.factorValues.Length; i++)
        {
            drainAmount -= (lifeFactors.factorValues[i]) * (factorWeights[i] / weightsTotal) * hours * hourlyDrainMax;
        }

        //Debug.Log($"{hours} hours passed. motivation drained by {drainAmount}");

        playerStats.ChangeMotivation(-drainAmount);
    }

}
