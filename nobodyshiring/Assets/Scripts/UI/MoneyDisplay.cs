using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    Money money;

    [SerializeField] TextMeshProUGUI moneyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = Money.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = $"money: ${money.moneyTotal}";
    }
}
