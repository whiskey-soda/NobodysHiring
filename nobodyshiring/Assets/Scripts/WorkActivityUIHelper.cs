using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class WorkActivityUIHelper : ActivityUIHelper
{
    [SerializeField] TMP_Dropdown projectDropdown;

    private void Start()
    {
        WorkManager.Instance.workListUpdated.AddListener(UpdateDropdown);

        UpdateDropdown();
    }

    public override void DoActivity()
    {
        if (activity as WorkActivity != null)
        {
            ((WorkActivity)activity).DoActivity(desiredDuration, WorkManager.Instance.projects.ElementAt(projectDropdown.value));
        }
    }

    void UpdateDropdown()
    {
        projectDropdown.ClearOptions();

        List<string> projectNames = new List<string>();
        foreach (Project project in WorkManager.Instance.projects)
        {
            if (project is Job)
            {
                projectNames.Add(((Job)project).taskName);
            }
            else if (project is Gig)
            {
                projectNames.Add(((Gig)project).gigName);
            }
        }

        projectDropdown.AddOptions(projectNames);

    }
}
