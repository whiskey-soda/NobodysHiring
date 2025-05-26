using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Statbars : MonoBehaviour
{
    [SerializeField] RectTransform energyBar;
    [SerializeField] RectTransform motivationBar;
    [Space]
    [SerializeField] float barChangeSpeed = 40;
    [SerializeField] float energyMaxDisplayVal = 120;
    [SerializeField] float motivMaxDisplayVal = 100;

    float energyDisplayValue;
    float motivationDisplayValue;

    PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance;

        energyDisplayValue = playerStats.energy;
        motivationDisplayValue = playerStats.motivation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplayValue(ref energyDisplayValue, playerStats.energy);
        UpdateDisplayValue(ref motivationDisplayValue, playerStats.motivation);

        UpdateBarSizes();
    }

    /// <summary>
    /// tries to align display value with a real stat value
    /// </summary>
    /// <param name="displayValue"></param>
    /// <param name="statValue"></param>
    private void UpdateDisplayValue(ref float displayValue, float statValue)
    {
        if (displayValue != statValue)
        {
            // display value is less than the change per frame, set it equal
            if (Mathf.Abs(displayValue - statValue) <= barChangeSpeed * Time.deltaTime) { displayValue = statValue; }

            else
            {
                // add speed value to get display closer to real value
                displayValue += Mathf.Sign(statValue - displayValue) * barChangeSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// resizes stat bars x scale based on their display values
    /// </summary>
    void UpdateBarSizes()
    {
        energyBar.localScale = new Vector3(energyDisplayValue/energyMaxDisplayVal, energyBar.localScale.y, energyBar.localScale.z);
        motivationBar.localScale = new Vector3(motivationDisplayValue/motivMaxDisplayVal, motivationBar.localScale.y, motivationBar.localScale.z);
    }

}
