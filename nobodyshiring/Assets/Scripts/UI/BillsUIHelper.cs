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
    [Space(2)]
    [Header("utilities")]
    [SerializeField] TextMeshProUGUI utilitiesCost;
    [SerializeField] TextMeshProUGUI utilitiesBudget;
    [SerializeField] TextMeshProUGUI utilitiesDueDate;
    [Space(2)]
    [Header("groceries")]
    [SerializeField] TextMeshProUGUI groceriesCost;
    [SerializeField] TextMeshProUGUI groceriesBudget;
    [SerializeField] TextMeshProUGUI groceriesDueDate;

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

}
