using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] string displayText = "MONEY: $";
    [SerializeField] TextMeshProUGUI textComponent;
    Money moneyScript;

    void Start()
    {
        moneyScript = Money.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        textComponent.text = displayText + moneyScript.moneyTotal.ToString();
    }
}
