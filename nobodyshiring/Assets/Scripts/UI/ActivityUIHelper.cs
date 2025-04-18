using UnityEngine;

public class ActivityUIHelper : MonoBehaviour
{
    public float desiredDuration;

    [SerializeField] protected Activity activity;

    /// <summary>
    /// sets the desired duration for the activity. created for use with sliders
    /// </summary>
    /// <param name="value"></param>
    public void SetDesiredDuration(float value)
    {
        desiredDuration = value;
    }

    /// <summary>
    /// triggers the linked activity for the desired duration float value.
    /// created for use with "do activity" buttons
    /// </summary>
    public virtual void DoActivity()
    {
        activity.DoActivity(desiredDuration);
    }
}
