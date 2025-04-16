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
        List<WorkOffer> expiredOffers = new List<WorkOffer>();
        foreach (WorkOffer offer in availableOffers)
        {
            // all jobs removed the next day
            if (offer is JobOffer)
            {
                expiredOffers.Add(offer);
            }
            // gigs remain for multiple days
            else if (offer is GigOffer)
            {
                (offer as GigOffer).daysUntilRemoval--;
                if ((offer as GigOffer).daysUntilRemoval <= 0)
                {
                    expiredOffers.Add(offer);
                }
            }
        }
        // remove expired offers from available offers list
        foreach (WorkOffer offer in expiredOffers)
        {
            availableOffers.Remove(offer);
        }


        List<WorkOffer> recievedOffers = new List<WorkOffer>();
        // decrement daysUntilArrival for all upcoming offers,
        // and move them into the availableOffers list when appropriate
        foreach (WorkOffer offer in upcomingOffers)
        {
            offer.daysUntilArrival--;
            if (offer.daysUntilArrival <= 0)
            {
                availableOffers.Add(offer);
                recievedOffers.Add(offer);
            }
        }
        //remove recieved offers from upcoming offers list
        foreach (WorkOffer offer in recievedOffers)
        {
            upcomingOffers.Remove(offer);
        }

    }

    public void AddWorkOffer(JobOffer newJobOffer)
    {
        upcomingOffers.Add(newJobOffer);
    }

    public void AddWorkOffer(GigOffer newGigOffer)
    {
        upcomingOffers.Add(newGigOffer);
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
