using UnityEngine;

public class ApplyForJobs : Activity
{
    public override void DoActivity(float duration)
    {
        base.DoActivity(duration);

        duration = ClampDuration(duration);
        LookForWork.Instance.ApplyForJobs(duration);
        
    }
}
