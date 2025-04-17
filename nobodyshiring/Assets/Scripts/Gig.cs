using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Gig : Project
{
    [Space]
    public string gigName;

    public List<Task> stages = new List<Task>();
    public Task currentStage;


    public Gig(GigData data)
    {
        Init(data);
    }
    
    /// <summary>
    /// configures gig with data from SO, and creates + configures tasks for the stages
    /// </summary>
    /// <param name="data"></param>
    private void Init(GigData data)
    {
        gigName = data.gigName;

        foreach (TaskData stageData in data.stages)
        {
            stages.Add(new Task(stageData));
        }

        currentStage = stages.First();

        // update current stages whenever the work list gets updated
        WorkManager.Instance.workListUpdated.AddListener(UpdateCurrentStage);
    }

    private void UpdateCurrentStage()
    {
        if (currentStage.complete)
        {
            if (stages.Last() == currentStage)
            {
                // all stages complete
                CompleteGig();
            }
            else
            {
                // complete current stage and update current stage
                currentStage = stages.ElementAt(stages.IndexOf(currentStage) + 1);
            }
        }
    }

    void CompleteGig()
    {

    }

}
