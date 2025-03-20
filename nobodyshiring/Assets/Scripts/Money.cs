using UnityEngine;

public class Money : MonoBehaviour
{

    public static Money Instance;

    float money;

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
        return price < money;
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }

    public void SubtractMoney(float amount)
    {
        if ( CanAfford(amount) )
        {

            money -= amount;
        }
    }

}
