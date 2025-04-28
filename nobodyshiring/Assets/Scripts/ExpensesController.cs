using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum Expense { rent, utilities, groceries }

public class ExpensesController : MonoBehaviour
{
    Money money;
    TimeTracking time;
    LifeFactors lifeFactors;

    [SerializeField] uint rentDueDay = 1;

    Date[] dueDates;
    float[] moneyDue;
    float[] budget;

    public UnityEvent billPastDue;

    public static ExpensesController Instance;
    private void Awake()
    {
        // singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        // initialize expense price/budget arrays
        int expenseCount = Enum.GetNames(typeof(Expense)).Count();
        dueDates = new Date[expenseCount];
        moneyDue = new float[expenseCount];
        budget = new float[expenseCount];

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = Money.Instance;
        time = TimeTracking.Instance;
        lifeFactors = LifeFactors.Instance;

        time.dayEnd.AddListener(CheckBillsDue);
    }

    /// <summary>
    /// runs as a day ends.
    /// checks to see if any bills were due in the past day, and tries to pay them.
    /// triggers an event if bills were not paid successfully.
    /// </summary>
    void CheckBillsDue()
    {
        bool billsPaid = true;

        // check if any expenses are due
        foreach (Date dueDate in dueDates)
        {
            if (dueDate.Equals(time.GetCurrentDate())) // if expense due
            {
                // try to pay expense
                Expense expense = (Expense)Array.IndexOf(dueDates, dueDate);
                if (TryPayExpense(expense) == false) { billsPaid = false; } // fail to pay expense, means bills not paid
            }
        }

        if (!billsPaid) { billPastDue.Invoke(); }
    }

    /// <summary>
    /// tries to pay a given expense with the budget.
    /// if that fails to cover the expense, tries to pay the difference from the player's wallet.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>true if expense was paid successfully. false if insufficient funds</returns>
    private bool TryPayExpense(Expense expense)
    {
        bool expensePaid = true;

        if (!ExpensePaid(expense)) // if any money is due
        {
            // try to pay from budget
            if (TryPayFromBudget(expense) == false) // not enough money in budget to cover expense
            {
                // try to pay from wallet
                if (TryPayFromMoney(expense) == false) { expensePaid = false; } // failed to pay expense
            }

            if (expense == Expense.rent && expensePaid)
            {
                lifeFactors.SetFactorValue(LifeFactor.UnpaidBills, 1);
            }
        }

        return expensePaid;
    }


    /// <summary>
    /// adds a given value of money to an expense's budget, and removes it from the player's money total
    /// </summary>
    /// <param name="expense"></param>
    /// <param name="moneyAmount"></param>
    public void AddToBudget(Expense expense, float moneyAmount)
    {
        // do nothing if player cannot affort to add this money to the budget
        if (!money.CanAfford(moneyAmount)) { return; }

        budget[(int)expense] += moneyAmount;
        money.SubtractMoney(moneyAmount);
    }

    /// <summary>
    /// tries to pay an expense with the money from the budget. removes the money used for the payment from the budget
    /// and adjusts amount due accordingly.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>returns true if expense was fully paid, false if there were inadequate funds in the budget</returns>
    public bool TryPayFromBudget(Expense expense)
    {
        bool expensePaid = false;

        int expenseInt = (int)expense;

        if (budget[expenseInt] >= moneyDue[expenseInt])
        {
            budget[expenseInt] -= moneyDue[expenseInt];
            moneyDue[expenseInt] = 0;
            expensePaid = true;
        }
        else
        {
            moneyDue[expenseInt] -= budget[expenseInt];
            budget[expenseInt] = 0;
        }

        return expensePaid;
    }

    /// <summary>
    /// tries to pay an expense with money from the player's money total. if insufficient funds, returns false and deducts no money.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>returns true if expense was fully paid, false if there were inadequate funds in the player's money total.</returns>
    public bool TryPayFromMoney(Expense expense)
    {
        bool expensePaid = false;

        int expenseInt = (int)expense;
        if (money.CanAfford(moneyDue[expenseInt])) // can afford expense
        {
            // pay off expense
            money.SubtractMoney(moneyDue[expenseInt]);
            expensePaid = true;
        }

        return expensePaid;
    }

    /// <summary>
    /// checks if a given expense has any money due.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>true if expense has been paid off (no money due). false if there is any money due.</returns>
    bool ExpensePaid(Expense expense)
    {
        return moneyDue[(int)expense] <= 0;
    }

}
