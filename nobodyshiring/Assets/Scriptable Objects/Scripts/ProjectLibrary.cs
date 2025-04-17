using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectLibrary", menuName = "Scriptable Objects/ProjectLibrary")]
public class ProjectLibrary : ScriptableObject
{
    public List<Project> projects = new List<Project>();

    /// <summary>
    /// returns a list of jobs contained in the project list
    /// </summary>
    /// <returns></returns>
    public List<Job> GetJobs()
    {
        List<Job> jobs = new List<Job>();
        foreach (Project project in projects)
        {
            Job job = project as Job;
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
    public List<Gig> GetGigs()
    {
        List<Gig> gigs = new List<Gig>();
        foreach (Project project in projects)
        {
            Gig gig = project as Gig;
            if (gig != null)
            {
                gigs.Add(gig);
            }
        }
        return gigs;
    }
}
