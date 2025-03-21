using UnityEngine;

public class GigStage : Task
{
    [Space]
    [SerializeField] float pay;

    /// <summary>
    /// pays the player for the completed gig stage
    /// </summary>
    protected override void Complete()
    {
        base.Complete();
        Money.Instance.AddMoney(pay);
    }
}
