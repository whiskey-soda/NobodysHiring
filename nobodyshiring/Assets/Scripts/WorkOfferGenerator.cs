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
    //[SerializeField] uint jobOfferChance;
    //[SerializeField] uint gigOfferChance;
    //[Space]
    [SerializeField] uint[] gigOfferDaysToArriveRange = new uint[2];
    [SerializeField] uint[] jobOfferDaysToArriveRange = new uint[2];
    [Space]
    [SerializeField]
    List<Job> jobs = new List<Job>();
    [SerializeField]
    List<Gig> gigs = new List<Gig>();


    GigOffer CreateGigOffer()
    {
        Gig gig = gigs[Random.Range(0, gigs.Count)];
        uint arrivalDelay = (uint)Random.Range((int)gigOfferDaysToArriveRange.First(), (int)gigOfferDaysToArriveRange.Last());

        return new GigOffer(gig, (int)arrivalDelay);
    }

    JobOffer CreateJobOffer()
    {
        Job job = jobs[Random.Range(0, jobs.Count)];
        uint arrivalDelay = (uint)Random.Range((int)jobOfferDaysToArriveRange.First(), (int)jobOfferDaysToArriveRange.Last());

        return new JobOffer(job, (int)arrivalDelay);
    }
}
