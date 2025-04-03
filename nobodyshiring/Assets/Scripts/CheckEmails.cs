using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CheckEmails : Activity
{
    class Offer
    {
        public int daysUntilArrival;
    }
    class JobOffer : Offer
    {
        public Job job;
    }
    class GigOffer : Offer
    {
        public Gig gig;
        public int daysUntilRemoval = 2;
    }

    List<Offer> inbox = new List<Offer>();
    List<Offer> upcomingOffers = new List<Offer>();

    protected override void Start()
    {
        base.Start();
        TimeTracking.Instance.dayEnd.AddListener(UpdateInbox);
    }

    public override void DoActivity(float duration)
    {
        base.DoActivity(duration);

    }

    void UpdateInbox()
    {
        foreach (Offer offer in inbox)
        {
            if (offer is JobOffer)
            {
                inbox.Remove(offer);
            }
            else if (offer is GigOffer)
            {
                (offer as GigOffer).daysUntilRemoval--;
                if ((offer as GigOffer).daysUntilRemoval <= 0)
                {
                    inbox.Remove(offer);
                }
            }
        }
    }
}
