using UnityEngine;

public class ActivityMenu : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnMouseEnter()
    {
        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Opening");
    }

    private void OnMouseExit()
    {
        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Closing");
    }
}
