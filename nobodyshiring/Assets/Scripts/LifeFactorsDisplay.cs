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
        factorsText.text = $"life factors\n";
        for (int i = 0; i < factors.factorCoefficients.Length; i++)
        {
            factorsText.text += $"{(LifeFactor)i}: {factors.factorCoefficients[i]}\n";
        }
    }
}
