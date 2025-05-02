using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    SleepManager sleepManager;
    Thermostat thermostat;

    public float motivationMax { get; private set; } = 100;
    public float energyMax { get; private set; } = 100;

    public float motivation { get; private set; } = 100;
    public float energy { get; private set; } = 100;


    [SerializeField] float highMotivationThresholdPercent = .7f;
    [SerializeField] float lowMotivationThresholdPercent = .25f;
    public float lowMotivationThreshold { get; private set; }
    public float highMotivationThreshold { get; private set; }


    [SerializeField] float highEnergyThresholdPercent = .7f;
    [SerializeField] float lowEnergyThresholdPercent = .25f;
    public float lowEnergyThreshold { get; private set; }
    public float highEnergyThreshold { get; private set; }



    [SerializeField] float highMotivationEnergyCostMultMAX = .25f;
    [SerializeField] float lowMotivationEnergyCostMultMAX = 2;

    [Space]
    [SerializeField] float passOutEnergyThreshold = 5;

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
        energy = energyMax;

        lowMotivationThreshold = lowMotivationThresholdPercent * motivationMax;
        highMotivationThreshold = highMotivationThresholdPercent * motivationMax;

        lowEnergyThreshold = lowEnergyThresholdPercent * energyMax;
        highEnergyThreshold = highEnergyThresholdPercent * energyMax;
    }

    private void Start()
    {
        sleepManager = SleepManager.Instance;
        thermostat = Thermostat.Instance;
    }

    /// <summary>
    /// modifies the player's motivation by a given value, factoring in thermostat affects
    /// </summary>
    /// <param name="motivationChange"></param>
    public void ChangeMotivation(float motivationChange)
    {
        // if motivation is being lost, cost gets modified
        if (Mathf.Sign(motivationChange) < 0)
        {
            // thermostat applies multiplier
            if (!thermostat.On) { motivationChange *= thermostat.motivationCostMult; }
        }

        motivation += motivationChange;
    }

    /// <summary>
    /// modifies the player's motivation, with a parameter option to use/ignore thermostat affects
    /// </summary>
    /// <param name="motivationChange"></param>
    /// <param name="considerThermostat"></param>
    public void ChangeMotivation(float motivationChange, bool considerThermostat)
    {
        // if motivation is being lost, cost gets modified
        if (considerThermostat && Mathf.Sign(motivationChange) < 0)
        {
            // thermostat applies multiplier
            if (!thermostat.On) { motivationChange *= thermostat.motivationCostMult; }
        }

        motivation += motivationChange;
    }

    /// <summary>
    /// changes the players energy, accounting for motivation influence.
    /// </summary>
    /// <param name="energyChange"></param>
    /// <returns>the proportion of energy that was used before passing out.</returns>
    public float ChangeEnergy(float energyChange)
    {
        // if energy is being lost, cost gets modified
        if (Mathf.Sign(energyChange) < 0) 
        {
            // high/low motivation multiplies the energy cost
            energyChange = CalculateMotivationInfluence(energyChange); 

            // thermostat applies multiplier
            if (!thermostat.On) { energyChange *= thermostat.energyCostMult; }
        }

        energy += energyChange;

        float proportionEnergyUsed = 1;

        if (energy <= passOutEnergyThreshold)
        {
            sleepManager.PassOut();

            proportionEnergyUsed = (energyChange - energy) / energyChange;
        }

        return proportionEnergyUsed;
    }

    public void SetEnergy(float value)
    {
        energy = value;
    }

    /// <summary>
    /// adjusts energy cost up or down based on current motivation levels
    /// </summary>
    /// <param name="energyCost"></param>
    /// <returns></returns>
    private float CalculateMotivationInfluence(float energyCost)
    {
        // at low motivation, energy costs ramp up to a maximum multiplier.
        // at high motivation, energy costs decrease to a minimum multiplier.

        float adjustedEnergyCost = energyCost;

        if (motivation < lowMotivationThreshold)
        {
            float interpValue = (lowMotivationThreshold - motivation) / lowMotivationThreshold;
            adjustedEnergyCost *= Mathf.Lerp(1, lowMotivationEnergyCostMultMAX, interpValue);

            /* 
            Debug.Log($"low motivation is making this cost more energy.\n" +
                $"original cost: {energyCost}\n" +
                $"adjusted cost: {adjustedEnergyCost}\n" +
                $"coefficient: {Mathf.Lerp(1, lowMotivationEnergyCostMultMAX, interpValue)}");
            */
        }
        else if (motivation > highMotivationThreshold)
        {
            float interpValue = (motivation - highMotivationThreshold) / (motivationMax - highMotivationThreshold);
            adjustedEnergyCost *= Mathf.Lerp(1, highMotivationEnergyCostMultMAX, interpValue);

            /*
            Debug.Log($"high motivation is making this cost less energy.\n" +
                $"original cost: {energyCost}\n" +
                $"adjusted cost: {adjustedEnergyCost}\n" +
                $"coefficient: {Mathf.Lerp(1, lowMotivationEnergyCostMultMAX, interpValue)}");
            */
        }

        return adjustedEnergyCost;
    }
}
