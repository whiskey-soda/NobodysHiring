using UnityEngine;

public class LookForWork : MonoBehaviour
{
    //TODO: rename these variables. offer success chance means the chance that
    // the job was offered. maybe will rename them during job search rework?
    
    [Header("GIGS")]
    [SerializeField] uint gigOfferChancesPerHour = 3;
    [SerializeField] float gigOfferSuccessChance = .2f;

    [Space]

    [Header("JOBS")]
    [SerializeField] uint jobOfferChancesPerHour = 2;
    [SerializeField] float jobOfferSuccessChance = .02f;

    WorkOfferGenerator offerGenerator;
    Inbox inbox;

    private void Start()
    {
        offerGenerator = WorkOfferGenerator.Instance;
        inbox = Inbox.Instance;
    }

    public void LookForGigs(float hours)
    {
        uint offersFound = RNG_FindOffers(hours, gigOfferChancesPerHour, gigOfferSuccessChance);

        for (uint i = 0; i < offersFound; i++)
        {
            inbox.AddWorkOffer(offerGenerator.GenerateGigOffer());
        }

    }

    public void ApplyForJobs(float hours)
    {
        uint offersFound = RNG_FindOffers(hours, jobOfferChancesPerHour, jobOfferSuccessChance);

        for (uint i = 0; i < offersFound; i++)
        {
            inbox.AddWorkOffer(offerGenerator.GenerateJobOffer());
        }
    }

    /// <summary>
    /// uses random chance to provide a number of offers based on hours spent searching
    /// </summary>
    /// <param name="hours"></param>
    private uint RNG_FindOffers(float hours, uint offerChancesPerHour, float offerSuccessChance)
    {
        uint offersFound = 0;

        uint possibleOffers = (uint)(hours * offerChancesPerHour); // rounds towards zero when casting

        for (uint i = 0; i < possibleOffers; i++)
        {
            // roll under, meets beats
            if (Random.Range(0f, 1f) <= offerSuccessChance)
            {
                offersFound++;
            }
        }

        return offersFound;
    }



}
