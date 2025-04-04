using System.Collections.Generic;
using UnityEngine;

public class WorkOffer
{
    public int daysUntilArrival;
}
public class JobOffer : WorkOffer
{
    public Job job;

    public JobOffer(Job job, int daysUntilArrival)
    {
        this.job = job;
        this.daysUntilArrival = daysUntilArrival;
    }
}
public class GigOffer : WorkOffer
{
    public Gig gig;
    public int daysUntilRemoval = 2;

    public GigOffer(Gig gig, int daysUntilArrival)
    {
        this.gig = gig;
        this.daysUntilRemoval = daysUntilArrival;
    }
}

public class Inbox : MonoBehaviour
{
    public List<WorkOffer> AvailableOffers { get; private set; } = new List<WorkOffer>();
    List<WorkOffer> upcomingOffers = new List<WorkOffer>();


    public static Inbox Instance;

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

    private void Start()
    {
        TimeTracking.Instance.dayEnd.AddListener(UpdateInbox);
    }

    void UpdateInbox()
    {
        foreach (WorkOffer offer in AvailableOffers)
        {
            // all jobs removed the next day
            if (offer is JobOffer)
            {
                AvailableOffers.Remove(offer);
            }
            // gigs remain for multiple days
            else if (offer is GigOffer)
            {
                (offer as GigOffer).daysUntilRemoval--;
                if ((offer as GigOffer).daysUntilRemoval <= 0)
                {
                    AvailableOffers.Remove(offer);
                }
            }
        }

        // decrement daysUntilArrival for all upcoming offers,
        // and move them into the availableOffers list when appropriate
        foreach (WorkOffer offer in upcomingOffers)
        {
            offer.daysUntilArrival--;
            if (offer.daysUntilArrival <= 0)
            {
                AvailableOffers.Add(offer);
                upcomingOffers.Remove(offer);
            }
        }

    }

    public void AddWorkOffer(Job job, int daysUntilArrival)
    {
        AvailableOffers.Add(new JobOffer(job, daysUntilArrival));
    }

    public void AddWorkOffer(Gig gig, int daysUntilArrival)
    {
        AvailableOffers.Add(new GigOffer(gig, daysUntilArrival));
    }

}
