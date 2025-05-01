using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum Expense { rent, utilities, groceries }

public class ExpensesController : MonoBehaviour
{
    [System.Serializable]
    struct DueDate
    {
        public Expense expense;
        public Date date;
    }

    #region variables

    [SerializeField]
    List<DueDate> expenseDueDates = new List<DueDate>();

    [Space]
    [Header("Rent")]
    [SerializeField] Date escrowChangeDate; // yearly date when rent has a chance to increase

    [Tooltip("chance for escrow to raise each year")]
    [SerializeField] float escrowRaiseChance = .7f;
    [Tooltip("minimum percent that rent price will increase")]
    [SerializeField] float escrowIncreaseMin = .02f;
    [Tooltip("maximum percent that rent price will increase")]
    [SerializeField] float escrowIncreaseMax = .06f;

    [Space]
    [Header("Utilities")]
    [SerializeField] float utilitiesBaseCost = 300;
    [SerializeField] float utilityBaseVarianceMin = .9f;
    [SerializeField] float utilityBaseVarianceMax = 1.1f;

    [SerializeField] float thermostatHourlyCostMin = .19f;
    [SerializeField] float thermostatHourlyCostMax = .23f;

    [Space]
    [Header("Groceries")]
    // amount that grocery cost increases each day
    [Tooltip("MIN amount that grocery cost increases each day")]
    [SerializeField] float groceryDailyCostMax = 15;
    [Tooltip("MAX amount that grocery cost increases each day")]
    [SerializeField] float groceryDailyCostMin = 22;

    // days until grocery expense is due
    [Tooltip("MIN days until grocery expense is due")]
    [SerializeField] uint groceryDueIntervalMin = 16;
    [Tooltip("MAX days until grocery expense is due")]
    [SerializeField] uint groceryDueIntervalMax = 21;

    // days until player incurs motivation penalty from groceries.
    // motivation drains from unexciting and bad meals
    [Tooltip("days until player incurs motivation penalty from groceries")]
    [SerializeField] uint groceryPenaltyThreshold = 10;

    [Space]
    public UnityEvent billPastDue;

    Date[] dueDates;
    float[] moneyDue;
    float[] budget;

    Money money;
    TimeTracking time;
    LifeFactors lifeFactors;

    public static ExpensesController Instance;

    #endregion

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

        foreach (DueDate dueDate in expenseDueDates)
        {
            dueDates[(int)dueDate.expense] = dueDate.date;
        }

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

    /// <summary>
    /// has a chance to raise rent price each year due to escrow increasing
    /// </summary>
    void YearlyEscrowChange()
    {
        if (time.GetCurrentDate().Equals(escrowChangeDate))
        {
            // roll under
            if (UnityEngine.Random.Range(0f, 1f) < escrowRaiseChance )
            {
                moneyDue[(int)Expense.rent] *= 1 + UnityEngine.Random.Range(escrowIncreaseMin, escrowIncreaseMax);
            }
        }
    }
}
