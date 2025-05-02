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
    [SerializeField] float rentPrice = 2600;
    
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

    float thermostatCost = 0;

    [Space]
    [Header("Groceries")]
    // amount that grocery cost increases each day
    [Tooltip("MIN amount that grocery cost increases each day")]
    [SerializeField] float groceryDailyCostMin = 15;
    [Tooltip("MAX amount that grocery cost increases each day")]
    [SerializeField] float groceryDailyCostMax = 22;

    // days until grocery expense is due
    [Tooltip("MIN days until grocery expense is due")]
    [SerializeField] uint groceryDueIntervalMin = 16;
    [Tooltip("MAX days until grocery expense is due")]
    [SerializeField] uint groceryDueIntervalMax = 21;

    // days until player incurs motivation penalty from groceries.
    // motivation drains from unexciting and bad meals
    [Tooltip("days until player incurs motivation penalty from groceries")]
    [SerializeField] uint groceryPenaltyThreshold = 10;
    [Tooltip("proportion of grocery cost needed to buy enough food to survive")]
    [SerializeField] float survivalGroceryProportion = .6f;
    uint daysSinceGroceryShopping = 0;

    [Space]
    public UnityEvent billPastDue;

    public Date[] dueDates { get; private set; }
    public float[] moneyDue { get; private set; }
    public float[] budget {  get; private set; }

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
        time.dayEnd.AddListener(DailyGroceryPriceIncrease);
        time.dayEnd.AddListener(UpdateGroceryLifeFactor);
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

                // RENT
                if (expense == Expense.rent)
                {
                    if (TryPayExpense(expense) == true) // try to pay rent with budget AND wallet money, since its due today
                    {
                        // reset price and life factor for new month
                        moneyDue[(int)expense] = rentPrice;
                        lifeFactors.SetFactorValue(LifeFactor.UnpaidBills, 0);
                    }
                    else { billsPaid = false; } // rent payment failed
                }

                // UTILITIES
                else if (expense == Expense.utilities)
                {
                    CalculateUtilitiesPrice(); // price calculated now since it depends on thermostat usage for entire month
                    if (TryPayExpense(expense) == false) { billsPaid = false; } // utilities payment failed
                }

                // GROCERIES
                else if (expense == Expense.groceries)
                {
                    // try to buy all groceries
                    if (BuyAllGroceries() == false) // not enough money to buy all groceries
                    {
                        // not enough for all groceries, so try to buy survival groceries
                        if (BuySurvivalGroceries() == false) { billsPaid = false; } // groceries payment failed
                    }
                }
            }
        }

        if (!billsPaid) { billPastDue.Invoke(); }
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

    #region paying expenses

    /// <summary>
    /// if player can afford an expense, method tries to pay with the budget.
    /// if that fails to fully cover the expense, tries to pay the difference from the player's wallet.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>true if expense was paid successfully. false if insufficient funds</returns>
    private bool TryPayExpense(Expense expense)
    {
        // do nothing if expense is unaffordable
        if (!CanAfford(expense)) { return false; }

        bool expensePaid = true;

        if (!ExpensePaid(expense)) // if any money is due
        {
            // try to pay from budget
            if (PayFromBudget(expense) == false) // not enough money in budget to fully cover expense
            {
                // try to pay remainder from wallet
                if (TryPayFromMoney(expense) == false) { expensePaid = false; } // failed to pay expense, GAME OVER
            }
        }

        return expensePaid;
    }

    /// <summary>
    /// tries to pay the rent with ONLY the rent budget. if successful, sets the unpaidBills life factor accordingly.
    /// if the budget is insufficient, nothing happens.
    /// </summary>
    /// <returns>true if payment was successful. false if payment failed.</returns>
    bool TryPayRentEarly()
    {
        if (SufficientBudget(Expense.rent))
        {
            if (PayFromBudget(Expense.rent) == true) // paid successfully
            {
                lifeFactors.SetFactorValue(LifeFactor.UnpaidBills, 1);
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// tries to purchase all groceries using budget and wallet. if successful, updates life factor and grocery due date.
    /// if unsufficient funds, does not deduct any money.
    /// </summary>
    /// <returns>true if purchase was successful, false if purchase was unsuccessful</returns>
    bool BuyAllGroceries()
    {
        bool groceriesPurchased = false;

        if (CanAfford(Expense.groceries))
        {
            if (TryPayExpense(Expense.groceries)) // purchase successful
            {
                lifeFactors.SetFactorValue(LifeFactor.GroceryQuality, 1); // set life factor
                SetGroceryDueDate();
                groceriesPurchased = true;
            }
        }

        return groceriesPurchased;
    }

    /// <summary>
    /// purchases bare minimum survival groceries for a fraction of the total grocery cost.
    /// if successful, updates grocery due date.
    /// if funds are insufficient, nothing happens.
    /// </summary>
    /// <returns>true if purchase was succesful, false if unsuccessful</returns>
    bool BuySurvivalGroceries()
    {
        bool groceriesPurchased = false;

        float groceryCost = moneyDue[(int)Expense.groceries] * survivalGroceryProportion;
        float availableFunds = budget[(int)Expense.groceries] + money.moneyTotal;

        if (availableFunds >= groceryCost) // can afford grocery bill
        {
            // if budget can afford the grocery bill...
            if (budget[(int)Expense.groceries] >= groceryCost)
            {
                budget[(int)Expense.groceries] -= groceryCost; // pay in full with budget
                groceriesPurchased = true; // success
            }
            else // insufficient budget to cover bill
            {
                // pay with budget first
                groceryCost -= budget[(int)Expense.groceries];
                budget[(int)Expense.groceries] = 0;

                // pay remainder out of wallet
                if (money.moneyTotal > groceryCost)
                {
                    money.SubtractMoney(groceryCost);
                    groceriesPurchased = true; // success
                }
            }
        }

        if (groceriesPurchased) { SetGroceryDueDate(); }// no life factor change

        return groceriesPurchased; // returns false if groceries were not purchased successfully
    }

    /// <summary>
    /// tries to pay an expense with the money from the budget. removes the money used for the payment from the budget
    /// and adjusts amount due accordingly.
    /// </summary>
    /// <param name="expense"></param>
    /// <returns>returns true if expense was fully paid, false if there were inadequate funds in the budget</returns>
    public bool PayFromBudget(Expense expense)
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
    /// checks if an expense's budget can cover its cost
    /// </summary>
    /// <param name="expense"></param>
    /// <returns></returns>
    bool SufficientBudget(Expense expense)
    {
        return budget[(int)expense] >= moneyDue[(int)expense];
    }

    /// <summary>
    /// checks if player can afford an expense, using budget and wallet money
    /// </summary>
    /// <param name="expense"></param>
    /// <returns></returns>
    bool CanAfford(Expense expense)
    {
        return budget[(int)expense] + money.moneyTotal > moneyDue[(int)expense];
    }

    #endregion

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
                rentPrice *= 1 + UnityEngine.Random.Range(escrowIncreaseMin, escrowIncreaseMax);
            }
        }
    }

    /// <summary>
    /// calculates and returns the utilities price for the month
    /// </summary>
    /// <returns></returns>
    float CalculateUtilitiesPrice()
    {
        float baseCost = utilitiesBaseCost * UnityEngine.Random.Range(utilityBaseVarianceMin, utilityBaseVarianceMax);
        return baseCost + thermostatCost;
    }

    /// <summary>
    /// raises the thermostat cost total for this month.
    /// raises by a random amount per hour used.
    /// </summary>
    /// <param name="duration"></param>
    public void UseThermostat(float duration)
    {
        thermostatCost += UnityEngine.Random.Range(thermostatHourlyCostMin, thermostatHourlyCostMax);
    }

    /// <summary>
    /// raises price of next grocery run by a daily amount
    /// </summary>
    void DailyGroceryPriceIncrease()
    {
        moneyDue[(int)Expense.groceries] += UnityEngine.Random.Range(groceryDailyCostMin, groceryDailyCostMax);
    }

    /// <summary>
    /// sets the grocery due date to a future date (determined by config parameters)
    /// </summary>
    void SetGroceryDueDate()
    {
        dueDates[(int)Expense.groceries].MoveDate((uint)UnityEngine.Random.Range(groceryDueIntervalMin, groceryDueIntervalMax));
    }

    /// <summary>
    /// registers a day since grocery shopping.
    /// updates the groceryQuality life factor based on how long its been since the player shopped for groceries
    /// </summary>
    void UpdateGroceryLifeFactor()
    {
        daysSinceGroceryShopping++;

        if (daysSinceGroceryShopping >= groceryPenaltyThreshold)
        {
            lifeFactors.SetFactorValue(LifeFactor.GroceryQuality, 0);
        }
        else { lifeFactors.SetFactorValue(LifeFactor.GroceryQuality, 1); }
    }

}
