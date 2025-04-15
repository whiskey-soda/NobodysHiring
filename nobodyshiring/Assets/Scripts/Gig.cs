using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Gig : MonoBehaviour
{
    [Space]
    public string gigName;

    public List<GigStage> stages = new List<GigStage>();
    public GigStage currentStage;


    private void Awake()
    {
        currentStage = stages.First();
    }

    private void Update()
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
