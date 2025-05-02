using UnityEngine;

public class Money : MonoBehaviour
{

    public static Money Instance;

    public float moneyTotal;

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

    public bool CanAfford(float price)
    {
        return price <= moneyTotal;
    }

    public void AddMoney(float amount)
    {
        moneyTotal += amount;
    }

    public void SubtractMoney(float amount)
    {
        if ( CanAfford(amount) )
        {

            moneyTotal -= amount;
        }
    }

}
