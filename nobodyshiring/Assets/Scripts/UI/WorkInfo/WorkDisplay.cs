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
        PopulateWorkInfoDisplay();
    }

    void PopulateWorkInfoDisplay()
    {
        DeleteAllWorkInfoPanels();
        CreateWorkInfoPanels();
    }

    void CreateWorkInfoPanels()
    {
        foreach (Project project in workManager.projects)
        {
            if (project is Job)
            {
                GameObject jobInfoPanel = Instantiate(jobPanel, workInfoDisplayParentTransform);
                jobInfoPanel.GetComponent<JobInfoDisplay>().Configure((Job)project);
            }
            else if (project is Gig)
            {
                GameObject gigInfoPanel = Instantiate(gigPanel, workInfoDisplayParentTransform);
                gigInfoPanel.GetComponent<GigInfoDisplay>().Configure((Gig)project);
            }
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
