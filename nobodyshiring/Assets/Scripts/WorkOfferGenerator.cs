using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#region offer classes
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
    [SerializeField]
    List<Job> jobs = new List<Job>();
    [SerializeField]
    List<Gig> gigs = new List<Gig>();

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
    /// generates a gig offer. randomly picks a gig from a list of gigs, 
    /// and pairs it with a random arrival delay based on config values.
    /// </summary>
    /// <returns></returns>
    public GigOffer GenerateGigOffer()
    {
        Gig gig = gigs[Random.Range(0, gigs.Count)];
        uint arrivalDelay = (uint)Random.Range((int)gigOfferDaysToArriveRange.First(), (int)gigOfferDaysToArriveRange.Last());

        return new GigOffer(gig, (int)arrivalDelay);
    }
    /// <summary>
    /// generates a job offer. randomly picks a job from a list of jobs,
    /// and pairs it with a random arrival delay based on confic values
    /// </summary>
    /// <returns></returns>
    public JobOffer GenerateJobOffer()
    {
        Job job = jobs[Random.Range(0, jobs.Count)];
        uint arrivalDelay = (uint)Random.Range((int)jobOfferDaysToArriveRange.First(), (int)jobOfferDaysToArriveRange.Last());

        return new JobOffer(job, (int)arrivalDelay);
    }
}
