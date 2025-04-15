using UnityEngine;
using System.Collections.Generic;

public class WorkDisplay : MonoBehaviour
{
    [SerializeField] GameObject gigPanel;
    [SerializeField] GameObject jobPanel;
    [SerializeField] Transform workInfoDisplayParentTransform;

    WorkManager workManager;
    private void Start()
    {
        workManager = WorkManager.Instance;

        workManager.workListUpdated.AddListener(PopulateWorkInfoDisplay);
    }

    void PopulateWorkInfoDisplay()
    {
        DeleteAllWorkInfoPanels();
        CreateWorkInfoPanels();
    }

    void CreateWorkInfoPanels()
    {
        foreach (Job job in workManager.jobs)
        {
            GameObject jobInfoPanel = Instantiate(jobPanel, workInfoDisplayParentTransform);
            jobInfoPanel.GetComponent<JobInfoDisplay>().Configure(job);
        }

        foreach (Gig gig in workManager.gigs)
        {
            GameObject gigInfoPanel = Instantiate(gigPanel, workInfoDisplayParentTransform);
            gigInfoPanel.GetComponent <GigInfoDisplay>().Configure(gig);
        }
    }

    void DeleteAllWorkInfoPanels()
    {
        foreach (Transform transform in workInfoDisplayParentTransform.GetComponentsInChildren<Transform>())
        {
            if (transform != workInfoDisplayParentTransform)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
