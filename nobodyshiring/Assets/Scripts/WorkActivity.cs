using UnityEngine;

public class WorkActivity : Activity
{
    public void DoActivity(float duration, Project project)
    {
        base.DoActivity(duration);

        if (project is Job)
        {
            ((Job)project).Work(duration);
        }
        else if (project is Gig)
        {
            ((Gig)project).currentStage.Work(duration);
        }
    }

}
