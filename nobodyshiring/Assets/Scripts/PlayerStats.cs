using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum PlayerStat { energy, motivation }

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

    [Space]
    [Tooltip("when the player ends the day by sleeping, if their energy is above this threshold percentage," +
        "they recieve a bonus to their motivation.")]
    [SerializeField] float motivationBoostEnergyThreshold = .3f;
    [SerializeField] float motivationBoostAmount = 8;

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
            if (!thermostat.isOn) { motivationChange *= thermostat.motivationCostMult; }
        }

        // cap motivation at max and 0 min
        float newMotivation = motivation + motivationChange;
        if (newMotivation > motivationMax) { newMotivation = motivationMax; }
        else if (newMotivation < 0) { newMotivation = 0; }

        motivation = newMotivation;
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
            // thermostat applies multiplier when off. uncomfortable temperatures make things take more effort
            if (!thermostat.isOn) { motivationChange *= thermostat.motivationCostMult; }
        }

        // cap motivation at max and 0 min
        float newMotivation = motivation + motivationChange;
        if (newMotivation > motivationMax) {  newMotivation = motivationMax; }
        else if (newMotivation < 0) {  newMotivation = 0; }

        motivation = newMotivation;
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
            if (!thermostat.isOn) { energyChange *= thermostat.energyCostMult; }
        }

        // cap energy at 0 min
        // dont cap at max because energy can increase over the cap with rest
        float newEnergy = energy + energyChange;
        if (newEnergy < 0) { newEnergy = 0; }
        energy = newEnergy;


        // calculate the proportion of energy used before passing out
        float proportionEnergyUsed = 1; // if player doesnt pass out, they use all the energy

        // if player passes out, return the proportion of energy that was used before passing out
        if (energy <= passOutEnergyThreshold)
        {
            sleepManager.PassOut();
            proportionEnergyUsed = (energyChange - energy) / energyChange;
        }

        return proportionEnergyUsed;
    }

    public void SetEnergy(float value)
    {
        // this does not check for caps because certain events, like well rested, can cause energy to go over the cap.
        // BUT this is purely a bonus, all calculations are still made with the 100 cap assumption
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

    /// <summary>
    /// if the player has a percentage of energy remaining, motivation is boosted.
    /// occurs when the player ends the day.
    /// </summary>
    public void ApplyMotivationBoostFromLeftoverEnergy()
    {
        bool significantEnergyRemaining = energy > motivationBoostEnergyThreshold * energyMax;

        // give motivation boost based on if energy was left over on previous day
        // simulates pacing yourself and maintaining your excitement for work
        if (significantEnergyRemaining)
        {
            ChangeMotivation(motivationBoostAmount);
        }
    }
}
