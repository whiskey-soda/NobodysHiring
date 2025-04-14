using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class WorkManager : MonoBehaviour
{
    public List<Gig> gigs { get; private set; } = new List<Gig>();
    public List<Job> jobs { get; private set; } = new List<Job>();

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
        if (!gigs.Contains(gig))
        {
            gigs.Add(gig);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool AddWork(Job job)
    {
        if (!jobs.Contains(job))
        {
            jobs.Add(job);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveWork (Gig gig)
    {
        if (gigs.Contains (gig))
        {
            gigs.Remove(gig);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveWork (Job job)
    {
        if (jobs.Contains(job))
        {
            jobs.Remove(job);
            workListUpdated.Invoke();
            return true;
        }
        return false;
    }

}
