using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class WorkManager : MonoBehaviour
{
    public List<Project> projects { get; private set; } = new List<Project>();
    //public List<Gig> gigs { get; private set; } = new List<Gig>();
    //public List<Job> jobs { get; private set; } = new List<Job>();

    public UnityEvent workListUpdated;

    public static WorkManager Instance;
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

    public bool AddWork(Gig gig)
    {
        if (!projects.Contains(gig))
        {
        projects.Add(gig);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool AddWork(Job job)
    {
        if (!projects.Contains(job))
        {
            projects.Add(job);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveWork (Gig gig)
    {
        if (projects.Contains (gig))
        {
            projects.Remove(gig);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveWork (Job job)
    {
        if (projects.Contains(job))
        {
            projects.Remove(job);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

}
