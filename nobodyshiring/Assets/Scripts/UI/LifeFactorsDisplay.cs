using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class LifeFactorsDisplay : MonoBehaviour
{

    LifeFactors factors;

    TextMeshProUGUI factorsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        factors = LifeFactors.Instance;
        factorsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        factorsText.text = $"(0 is bad, 1 is good)\n";
        for (int i = 0; i < factors.factorValues.Length; i++)
        {
            factorsText.text += $"{(LifeFactor)i}: {System.Math.Round(factors.factorValues[i], 2)}\n";
        }
    }
}
