using UnityEngine;

public class StressorBarMask : ProgressBarMask
{

    [Space]
    [SerializeField] LifeFactor stressor;
    [SerializeField] float stressorMax = 1;
    LifeFactors lifeFactorsScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeFactorsScript = LifeFactors.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // invert the stressor value because 1 is bad and 0 is good.
        // with inverted value, display shows full bar at 1 to signify that the stressor is really bad right now
        UpdateMaskRect(1- lifeFactorsScript.factorValues[(int)stressor], stressorMax);
    }
}
