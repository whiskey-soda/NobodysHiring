using System.Collections.Generic;
using UnityEngine;

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

    public void AddWorkOffer(JobOffer newJobOffer)
    {
        AvailableOffers.Add(newJobOffer);
    }

    public void AddWorkOffer(GigOffer newGigOffer)
    {
        AvailableOffers.Add(newGigOffer);
    }

}
