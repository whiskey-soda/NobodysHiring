using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectLibrary", menuName = "Scriptable Objects/ProjectLibrary")]
public class ProjectLibrary : ScriptableObject
{
    public List<ProjectData> projects = new List<ProjectData>();

    /// <summary>
    /// returns a list of jobs contained in the project list
    /// </summary>
    /// <returns></returns>
    public List<TaskData> GetJobs()
    {
        List<TaskData> jobs = new List<TaskData>();
        foreach (ProjectData project in projects)
        {
            TaskData job = project as TaskData;
            if (job != null)
            {
                jobs.Add(job);
            }
        }
        return jobs;
    }

    /// <summary>
    /// returns a list of gigs contained in the project list
    /// </summary>
    /// <returns></returns>
    public List<GigData> GetGigs()
    {
        List<GigData> gigs = new List<GigData>();
        foreach (ProjectData project in projects)
        {
            GigData gig = project as GigData;
            if (gig != null)
            {
                gigs.Add(gig);
            }
        }
        return gigs;
    }
}
