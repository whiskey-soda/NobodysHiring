using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillsUIHelper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rentText;
    [SerializeField] Button payRentButton;

    Bills bills;

    private void Start()
    {
        bills = Bills.Instance;
    }

    private void Update()
    {
        rentText.text = $"Rent/Mortgage: ${bills.rentPrice} ";
        if (bills.rentPaid)
        {
            rentText.text += "(PAID)";
        }
        else { rentText.text += "(UNPAID)"; }


        if (bills.rentPaid)
        {
            payRentButton.interactable = false;
        }
        else { payRentButton.interactable = true; }
    }

}
