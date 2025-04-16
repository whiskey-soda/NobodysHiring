using TMPro;
using UnityEngine;

public class WorkInfoDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI workName;

    public virtual void Configure(Gig gig)
    {

    }

    public virtual void Configure (Job job)
    {

    }
}
