using TMPro;
using UnityEngine;

public class WorkOfferUIHelper : MonoBehaviour
{
    protected WorkOffer offer;
    public virtual void Configure(WorkOffer workOffer)
    {
        
    }

    public void Accept()
    {

        if (offer is GigOffer)
        {
            Inbox.Instance.AcceptOffer((GigOffer)offer);
        }
        else if (offer is JobOffer)
        {
            Inbox.Instance.AcceptOffer((JobOffer)offer);
        }
    }

}
