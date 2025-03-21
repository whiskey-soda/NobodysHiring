using UnityEngine;

public class Job : Task
{

    [Space]
    [SerializeField] float pay;

    protected override void Start()
    {
        base.Start();

        TimeTracking.Instance.monthEnd.AddListener(Payday);
    }

    void Payday()
    {
        if (complete) { Money.Instance.AddMoney(pay); }
    }

}
