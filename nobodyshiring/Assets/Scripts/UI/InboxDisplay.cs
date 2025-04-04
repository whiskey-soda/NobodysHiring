using UnityEngine;

public class InboxDisplay : MonoBehaviour
{
    [SerializeField] GameObject inboxUI;
    [SerializeField] GameObject inboxContentParent;
    [Space]
    [SerializeField] GameObject workOfferUIPrefab;
    [SerializeField] float rectHeightIncreasePerLine = 30;

    Inbox inbox;

    private void Start()
    {
        inbox = Inbox.Instance;
    }

    public void ShowInbox()
    {
        inboxUI.SetActive(true);
        ClearInboxMenu();
        PopulateInboxMenu();
    }

    /// <summary>
    /// deletes all work offers in the inbox menu
    /// </summary>
    public void ClearInboxMenu()
    {
        foreach (Transform transform in inboxContentParent.GetComponentsInChildren<Transform>())
        {
            if (transform.gameObject != inboxContentParent)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void PopulateInboxMenu()
    {
        foreach (WorkOffer offer in inbox.AvailableOffers)
        {
            CreateWorkOfferUIPanel(offer);
        }
    }

    void CreateWorkOfferUIPanel(WorkOffer offer)
    {
        GameObject newWorkOfferUIObject = Instantiate(workOfferUIPrefab, inboxContentParent.transform);
        newWorkOfferUIObject.GetComponent<WorkOfferUIHelper>().Configure(offer);

        // extend panel to fit all relevant skills (one per line)
        if (offer is JobOffer) { ExtendSkillRecTextboxToFit(offer, newWorkOfferUIObject); }
    }

    /// <summary>
    /// extend panel to fit all relevant skills (one per line)
    /// </summary>
    /// <param name="offer"></param>
    private void ExtendSkillRecTextboxToFit(WorkOffer offer, GameObject workOfferUIObject)
    {

        // currently only used for jobs, since gigs have a sub-menu for stages
        int relevantSkillCount = 0;
        if (offer is JobOffer)
        {
            foreach (float skill in (offer as JobOffer).job.recommededSkillLevels)
            {
                if (skill != 0)
                {
                    relevantSkillCount++;
                }
            }
        }

        // extends panel to comfortably house all relevant skills in text box
        workOfferUIObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, rectHeightIncreasePerLine * relevantSkillCount);
    }
}
