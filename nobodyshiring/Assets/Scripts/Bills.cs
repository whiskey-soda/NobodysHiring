using UnityEngine;
using UnityEngine.Events;

public class Bills : MonoBehaviour
{
    Money money;
    TimeTracking time;
    LifeFactors lifeFactors;

    [SerializeField] uint rentDueDay = 1;
    public bool rentPaid { get; private set; } = false;

    public float rentPrice;

    public UnityEvent billPastDue;

    public static Bills Instance;
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
    /// checks to see if any bills were due in the past day, and if they are paid off.
    /// triggers an event if bills were not paid.
    /// </summary>
    void CheckBillsDue()
    {
        bool billsPaid = true;

        if (time.day == rentDueDay && rentPaid)
        {
            rentPaid = false;
            lifeFactors.ChangeFactorCoefficient(LifeFactor.UnpaidBills, -1);
        }
        // unpaid rent
        else
        {
            billsPaid = false;
        }

        if (!billsPaid) { billPastDue.Invoke(); }
    }

    /// <summary>
    /// subtracts money to pay rent.
    /// </summary>
    /// <returns></returns>
    public void PayRent()
    {
        if (money.CanAfford(rentPrice))
        {
            money.SubtractMoney(rentPrice);
            rentPaid = true;

            lifeFactors.ChangeFactorCoefficient(LifeFactor.UnpaidBills, 1);

        }

    }

}
