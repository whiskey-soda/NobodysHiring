using UnityEngine;

public class WorkActivity : Activity
{
    [Space]
    [SerializeField] float progressMultiplier = 1;

    public void DoActivity(float duration, Project project)
    {
        base.DoActivity(duration);

        if (project is Job)
        {
            ((Job)project).Work(progressMultiplier * duration);
        }
        else if (project is Gig)
        {
            ((Gig)project).currentStage.Work(progressMultiplier * duration);
        }
    }

}
