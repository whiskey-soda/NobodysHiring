using UnityEngine;

[System.Serializable]
public class Job : Task
{
    public Job(TaskData data) : base(data) // job constructor
    {
    }

    // override to add payday event as listener
    protected override void Init(TaskData data)
    {
        base.Init(data);

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
