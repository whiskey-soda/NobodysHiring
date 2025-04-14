using System.Collections.Generic;
using UnityEngine;

public class Inbox : MonoBehaviour
{
    public List<WorkOffer> availableOffers { get; private set; } = new List<WorkOffer>();
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
        foreach (WorkOffer offer in availableOffers)
        {
            // all jobs removed the next day
            if (offer is JobOffer)
            {
                availableOffers.Remove(offer);
            }
            // gigs remain for multiple days
            else if (offer is GigOffer)
            {
                (offer as GigOffer).daysUntilRemoval--;
                if ((offer as GigOffer).daysUntilRemoval <= 0)
                {
                    availableOffers.Remove(offer);
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
                availableOffers.Add(offer);
                upcomingOffers.Remove(offer);
            }
        }

    }

    public void AddWorkOffer(JobOffer newJobOffer)
    {
        availableOffers.Add(newJobOffer);
    }

    public void AddWorkOffer(GigOffer newGigOffer)
    {
        availableOffers.Add(newGigOffer);
    }

    public void AcceptOffer(GigOffer gigToAccept)
    {
        // do nothing if gig is not in inbox
        if (!availableOffers.Contains(gigToAccept)) { return; }

        // if adding the work was successful, delete the offer
        if (WorkManager.Instance.AddWork(gigToAccept.gig))
        {
            availableOffers.Remove((WorkOffer)gigToAccept);
        }
    }

    public void AcceptOffer(JobOffer jobToAccept)
    {
        // do nothing if gig is not in inbox
        if (!availableOffers.Contains(jobToAccept)) { return; }

        // if adding the work was successful, delete the offer
        if (WorkManager.Instance.AddWork(jobToAccept.job))
        {
            availableOffers.Remove((WorkOffer)jobToAccept);
        }
    }

}
