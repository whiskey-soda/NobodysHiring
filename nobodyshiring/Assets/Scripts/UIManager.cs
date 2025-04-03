using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject activityUIPrefab;
    [SerializeField] GameObject gigUIPrefab;
    [SerializeField] GameObject jobUIPrefab;

    [Space]
    [SerializeField] GameObject activityUIContents;
    [SerializeField] int activityPanelSize = 250;

    public static UIManager Instance;

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


    public void AddActivity(Activity activity)
    {
        ChangeActivityUISize(activityPanelSize);
        Activity newActivity = Instantiate(activityUIPrefab, activityUIContents.transform).GetComponent<Activity>();
        newActivity = activity;
    }

    public void AddActivity(Gig gig)
    {
        ChangeActivityUISize(activityPanelSize);
        Gig newGig = Instantiate(gigUIPrefab, activityUIContents.transform).GetComponent<Gig>();
        newGig = gig;
    }

    public void AddActivity(Job job)
    {
        ChangeActivityUISize(activityPanelSize);
        Job newJob = Instantiate(jobUIPrefab, activityUIContents.transform).GetComponent<Job>();
        newJob = job;
    }

    public void RemoveActivity(GameObject activityUIPanel)
    {
        Destroy(activityUIPanel);
        ChangeActivityUISize(-activityPanelSize);
    }

    void ChangeActivityUISize(float newHeight)
    {
        activityUIContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, activityPanelSize);
    }

}
