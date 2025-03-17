using UnityEngine;

public class Housekeeping : Activity
{

    [SerializeField] float cleanlinessIncrease = .2f;

    public override void DoActivity()
    {
        base.DoActivity();
        LifeFactors.Instance.RaiseFactorCoefficient(LifeFactor.Cleanliness, cleanlinessIncrease);
    }

}
