using UnityEngine;

public class Job : Task
{

    [Space]
    public float pay;

    protected override void Start()
    {
        base.Start();

        // payday triggers at month end
        TimeTracking.Instance.monthEnd.AddListener(Payday);
    }

    /// <summary>
    /// triggers at the end of the month. pays player if job was completed, and resets completion status
    /// </summary>
    void Payday()
    {
        if (complete) { Money.Instance.AddMoney(pay); complete = false; }
    }

}
