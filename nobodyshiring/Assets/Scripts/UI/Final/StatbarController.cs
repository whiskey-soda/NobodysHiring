using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StatbarController : MonoBehaviour
{
    [SerializeField] PlayerStat stat;
    [Space]

    [SerializeField] RectTransform statbarRect;
    Image statbarImage; // fetched from rect
    Color statbarOriginalColor; // fetched from image

    [Space]
    [SerializeField] float barChangeSpeed = 40;
    [SerializeField] float maxDisplayVal = 100; // 100 for motivation, 120 for energy

    float statbarDisplayValue;

    [Space(2)]
    [SerializeField] RectTransform previewRect;
    Image previewImage; // fetched from rect
    [Space]
    [SerializeField] Transform foregroundParent;
    [SerializeField] Transform backgroundParent;
    [Space]
    [SerializeField] Color previewIncreaseColor = Color.green;
    [SerializeField] Color previewDecreaseColor = Color.red;

    // value displayed by preview bar
    float previewDisplayValue;

    // value that is being previewed (bars approach these values when they are activated)
    float previewedValue;


    PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        statbarImage = statbarRect.GetComponentInChildren<Image>();
        previewImage = previewRect.GetComponentInChildren<Image>();

        statbarOriginalColor = statbarImage.color;

        // set initial display value
        if (stat == PlayerStat.energy) { statbarDisplayValue = playerStats.energy; }
        else if (stat == PlayerStat.motivation) { statbarDisplayValue = playerStats.motivation; }
    }

    // Update is called once per frame
    void Update()
    {
        // fetch correct stat value based on stat field
        float statValue = 0;
        if (stat == PlayerStat.energy) { statValue = playerStats.energy; }
        else if (stat == PlayerStat.motivation) { statValue = playerStats.motivation; }

        // update actual statbar
        UpdateDisplayValue(ref statbarDisplayValue, statValue);
        // only update preview bar if the actual statbar is not changing
        if (statbarDisplayValue == statValue) { UpdateDisplayValue(ref previewDisplayValue, previewedValue); }
        

        UpdateBarSizes();

        // if preview bar is displayed, set layering according to display values
        if (previewImage.color.a > 0) { SetStatbarLayers(); }
    }

    /// <summary>
    /// gradually aligns display value with a given stat value
    /// </summary>
    /// <param name="displayValue"></param>
    /// <param name="statValue"></param>
    private void UpdateDisplayValue(ref float displayValue, float statValue)
    {
        // cap display values so bars dont overextend
        if (displayValue > maxDisplayVal) { displayValue = maxDisplayVal; }
        else if (displayValue < 0) { displayValue = 0; }

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
        // update stat bar size
        statbarRect.localScale = new Vector3(Mathf.Min(1, statbarDisplayValue / maxDisplayVal),
                                                    statbarRect.localScale.y, statbarRect.localScale.z);

        // update preview bar size
        previewRect.localScale = new Vector3(previewDisplayValue / maxDisplayVal, previewRect.localScale.y, previewRect.localScale.z);

    }

    /// <summary>
    /// makes preview bars opaque and configures preview bar values
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="previewValue"></param>
    public void ShowPreview(float previewValue)
    {
        // set preview values equal to current display values, so preview bars start at the current value and then change.
        // also set preview values to the desired preview values
        previewDisplayValue = statbarDisplayValue;
        previewedValue = previewValue;

        // since the preview display value is the same as the statbar display value,
        // this method call hides the preview statbar behind the normal stat bar as it appears
        SetStatbarLayers();
    }

    /// <summary>
    /// makes preview bar opaque and sets preview value to the current statbar display value, making the preview invisible but active.
    /// </summary>
    public void ShowPreview()
    {
        ShowPreview(statbarDisplayValue);
    }

    /// <summary>
    /// changes the layer order of the preview and stat bar depending on if the preview bar is showing an increase or decrease
    /// </summary>
    private void SetStatbarLayers()
    {
        if (previewDisplayValue < statbarDisplayValue) // previewing a stat decrease
        {
            // preview is in front of statbar, statbar color is red.
            // this makes the difference between preview and actual appear red to signify the decrease
            previewRect.SetParent(foregroundParent, true);
            statbarImage.color = previewDecreaseColor;
            previewImage.color = statbarOriginalColor;
        }
        else if (previewDisplayValue >= statbarDisplayValue) // previewing a stat increase
        {
            // preview is behind statbar and turns green.
            // this makes the previewed increase appears green
            previewRect.SetParent(backgroundParent, true);
            statbarImage.color = statbarOriginalColor;
            previewImage.color = previewIncreaseColor;
        }
    }

    public void SetPreviewValue(float value)
    {
        previewedValue = value;
    }

    /// <summary>
    /// hides preview bar and returns main bar color
    /// </summary>
    public void HidePreview()
    {
        // restore statbar original color
        if (statbarImage.color != statbarOriginalColor) { statbarImage.color = statbarOriginalColor; }

        // make preview bar fully transparent (invisible)
        Color previewColor = statbarOriginalColor;
        previewColor.a = 0;
        previewImage.color = previewColor;
    }
}
