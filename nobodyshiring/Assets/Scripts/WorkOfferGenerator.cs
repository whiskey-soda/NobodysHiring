using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#region offer classes
[System.Serializable]
public class WorkOffer
{
    public int daysUntilArrival;
}

[System.Serializable]
public class JobOffer : WorkOffer
{
    public Job job;

    public JobOffer(Job job, int daysUntilArrival)
    {
        this.job = job;
        this.daysUntilArrival = daysUntilArrival;
    }
}

[System.Serializable]
public class GigOffer : WorkOffer
{
    public Gig gig;
    public int daysUntilRemoval = 2;

    public GigOffer(Gig gig, int daysUntilArrival)
    {
        this.gig = gig;
        this.daysUntilArrival = daysUntilArrival;
    }
}

#endregion

public class WorkOfferGenerator : MonoBehaviour
{
    [SerializeField] uint[] gigOfferDaysToArriveRange = new uint[2];
    [SerializeField] uint[] jobOfferDaysToArriveRange = new uint[2];
    [Space]
    [SerializeField] ProjectLibrary projectLibrary;

    public static WorkOfferGenerator Instance;
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

    /// <summary>
    /// generates a gig offer. randomly picks a gig from the project library, 
    /// and pairs it with a random arrival delay based on config values.
    /// </summary>
    /// <returns></returns>
    public GigOffer GenerateGigOffer()
    {
        // get all gigs from the project library
        List<GigData> gigs = projectLibrary.GetGigs();

        // pick a random one and create an offer with a random arrival delay
        Gig gig = new Gig( gigs[Random.Range(0, gigs.Count)] );
        uint arrivalDelay = (uint)Random.Range((int)gigOfferDaysToArriveRange.First(), (int)gigOfferDaysToArriveRange.Last());

        return new GigOffer(gig, (int)arrivalDelay);
    }
    /// <summary>
    /// generates a job offer. randomly picks a job from the project library,
    /// and pairs it with a random arrival delay based on confic values
    /// </summary>
    /// <returns></returns>
    public JobOffer GenerateJobOffer()
    {
        // get all jobs from the project library
        List<TaskData> jobs = projectLibrary.GetJobs();

        // pick a random one and create an offer with a random arrival delay
        Job newJob = new Job( jobs[Random.Range(0, jobs.Count)] );
        uint arrivalDelay = (uint)Random.Range((int)jobOfferDaysToArriveRange.First(), (int)jobOfferDaysToArriveRange.Last());

        return new JobOffer(newJob, (int)arrivalDelay);
    }
}
