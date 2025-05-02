using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillsUIHelper : MonoBehaviour
{
    [Header("rent")]
    [SerializeField] TextMeshProUGUI rentCost;
    [SerializeField] TextMeshProUGUI rentBudget;
    [SerializeField] TextMeshProUGUI rentDueDate;
    [SerializeField] TMP_InputField rentInput;

    [Space(2)]
    [Header("utilities")]
    [SerializeField] TextMeshProUGUI utilitiesCost;
    [SerializeField] TextMeshProUGUI utilitiesBudget;
    [SerializeField] TextMeshProUGUI utilitiesDueDate;
    [SerializeField] TMP_InputField utilitiesInput;
    [Space(2)]
    [Header("groceries")]
    [SerializeField] TextMeshProUGUI groceriesCost;
    [SerializeField] TextMeshProUGUI groceriesBudget;
    [SerializeField] TextMeshProUGUI groceriesDueDate;
    [SerializeField] TMP_InputField groceriesInput;

    ExpensesController expensesController;

    private void Start()
    {
        expensesController = ExpensesController.Instance;
    }

    private void Update()
    {
        rentCost.text = "Cost: " + expensesController.moneyDue[(int)Expense.rent].ToString();
        rentBudget.text = "Budget: " + expensesController.budget[(int)Expense.rent].ToString();
        rentDueDate.text = "Due: " + expensesController.dueDates[(int)Expense.rent].ToString();



        utilitiesCost.text = "Cost: " + expensesController.moneyDue[(int)Expense.utilities].ToString();
        utilitiesBudget.text = "Budget: " + expensesController.budget[(int)(Expense.utilities)].ToString();
        utilitiesDueDate.text = "Due: " + expensesController.dueDates[(int)Expense.utilities].ToString();



        groceriesCost.text = "Cost: " + expensesController.moneyDue[(int)Expense.groceries].ToString();
        groceriesBudget.text = "Budget: " + expensesController.budget[(int)Expense.groceries].ToString();
        groceriesDueDate.text = "Due: " + expensesController.dueDates[(int)Expense.groceries].ToString();
    }

    public void RentBudget()
    {
        if (float.Parse(rentInput.text) <= 0) { return; } // invalid input, do not continue
        expensesController.AddToBudget(Expense.rent, float.Parse(rentInput.text));
    }

    public void PayRent()
    {
        expensesController.TryPayRentEarly();
    }

    public void UtilitiesBudget()
    {
        if (float.Parse(utilitiesInput.text) <= 0) { return; } // invalid input, do not continue
        expensesController.AddToBudget(Expense.utilities, float.Parse(utilitiesInput.text));
    }

    public void GroceriesBudget()
    {
        if (float.Parse(groceriesInput.text) <= 0) { return; } // invalid input, do not continue
        expensesController.AddToBudget(Expense.groceries, float.Parse(groceriesInput.text));
    }

    public void ShopGroceries()
    {
        expensesController.BuyAllGroceries();
    }

    public void ShopSurvivalGroceries()
    {
        expensesController.BuySurvivalGroceries();
    }

}
