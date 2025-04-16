using UnityEngine;

public class LookForClients : Activity
{
    public override void DoActivity(float duration)
    {
        base.DoActivity(duration);

        duration = ClampDuration(duration);
        LookForWork.Instance.LookForGigs(duration);

    }
}
