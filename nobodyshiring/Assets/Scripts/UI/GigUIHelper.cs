using UnityEngine;

public class GigUIHelper : WorkActivityUIHelper
{
    [SerializeField] Gig gig;

    private void Update()
    {
        task = gig.currentStage;
    }

}
