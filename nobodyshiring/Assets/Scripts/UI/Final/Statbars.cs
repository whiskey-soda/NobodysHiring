using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Statbars : MonoBehaviour
{
    [SerializeField] RectTransform energyBarRect;
    [SerializeField] RectTransform motivationBarRect;
    Image energyBarImage;
    Image motivationBarImage;

    [Space]
    [SerializeField] float barChangeSpeed = 40;
    [SerializeField] float energyMaxDisplayVal = 120;
    [SerializeField] float motivMaxDisplayVal = 100;

    float energyDisplayValue;
    float motivationDisplayValue;

    [Space]
    [SerializeField] RectTransform energyPreviewRect;
    [SerializeField] RectTransform motivationPreviewRect;
    Image energyPreviewImage;
    Image motivationPreviewImage;
    [SerializeField] float previewedAlpha = .6f;

    // values displayed by preview bars
    float energyPreviewDisplayValue;
    float motivationPreviewDisplayValue;

    // values that are to be previewed (bars approach these values when they are activated)
    float energyPreviewedValue;
    float motivationPreviewedValue;


    PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance;

        energyBarImage = energyBarRect.GetComponentInChildren<Image>();
        motivationBarImage = motivationBarRect.GetComponentInChildren<Image>();

        energyPreviewImage = energyPreviewRect.GetComponentInChildren<Image>();
        motivationPreviewImage = motivationPreviewRect.GetComponentInChildren<Image>();

        energyDisplayValue = playerStats.energy;
        motivationDisplayValue = playerStats.motivation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplayValue(ref energyDisplayValue, playerStats.energy);
        UpdateDisplayValue(ref motivationDisplayValue, playerStats.motivation);

        UpdateDisplayValue(ref energyPreviewDisplayValue, energyPreviewedValue);
        UpdateDisplayValue(ref motivationPreviewDisplayValue, motivationPreviewedValue);

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

            // hide previews if main bars hit the same value
            if (displayValue == energyDisplayValue && energyDisplayValue == energyPreviewDisplayValue) { HidePreview(PlayerStat.energy); }
            else if (displayValue == motivationDisplayValue && motivationDisplayValue == motivationPreviewDisplayValue) { HidePreview(PlayerStat.motivation); }

        }
    }

    /// <summary>
    /// resizes stat bars x scale based on their display values
    /// </summary>
    void UpdateBarSizes()
    {
        energyBarRect.localScale = new Vector3(Mathf.Min(1, energyDisplayValue / energyMaxDisplayVal), 
                                                    energyBarRect.localScale.y, energyBarRect.localScale.z);
        motivationBarRect.localScale = new Vector3(Mathf.Min(1, motivationDisplayValue / motivMaxDisplayVal), 
                                                    motivationBarRect.localScale.y, motivationBarRect.localScale.z);

        // update preview bar sizes
        energyPreviewRect.localScale = new Vector3(energyPreviewDisplayValue / energyMaxDisplayVal, energyPreviewRect.localScale.y, energyPreviewRect.localScale.z);
        motivationPreviewRect.localScale = new Vector3(motivationPreviewDisplayValue / motivMaxDisplayVal, motivationPreviewRect.localScale.y, motivationPreviewRect.localScale.z);

    }

    /// <summary>
    /// makes main stat bars slightly transparent, makes preview bars opaque, and configures preview bar values
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="previewValue"></param>
    public void ShowPreview(PlayerStat stat, float previewValue)
    {
        Image statbarImage;
        Image previewImage;

        // fetch appropriate image objects for specified stat
        if (stat == PlayerStat.energy)
        {
            statbarImage = energyBarImage;
            previewImage = energyPreviewImage;
        }
        else if (stat == PlayerStat.motivation)
        {
            statbarImage = motivationBarImage;
            previewImage = motivationPreviewImage;
        }
        else { return; } // failure to fetch player stat



        Color barColor = Color.white;

        // make main stat bar transparent
        barColor = statbarImage.color;
        barColor.a = previewedAlpha;
        statbarImage.color = barColor;

        // make preview bar opaque
        barColor = previewImage.color;
        barColor.a = 255;
        previewImage.color = barColor;

        // set preview values equal to current display values, so preview bars start at the current value and then change.
        // also set preview values to the desired preview values
        if (stat == PlayerStat.energy) { energyPreviewDisplayValue = energyDisplayValue; energyPreviewedValue = previewValue; }
        else if (stat == PlayerStat.motivation) { motivationPreviewDisplayValue = motivationDisplayValue; motivationPreviewedValue = previewValue; }
    }

    /// <summary>
    /// hides preview bar and returns main bar to full opacity for a given stat
    /// </summary>
    public void HidePreview(PlayerStat stat)
    {
        Image statbarImage;
        Image previewImage;

        // fetch appropriate image objects for specified stat
        if (stat == PlayerStat.energy)
        {
            statbarImage = energyBarImage;
            previewImage = energyPreviewImage;
        }
        else if (stat == PlayerStat.motivation)
        {
            statbarImage = motivationBarImage;
            previewImage = motivationPreviewImage;
        }
        else { return; } // failure to fetch player stat



        Color barColor = Color.white;


        // make main stat bar opaque
        barColor = statbarImage.color;
        barColor.a = 1;
        statbarImage.color = barColor;

        // make preview bar fully transparent
        barColor = previewImage.color;
        barColor.a = 0;
        previewImage.color = barColor;
    }

}
