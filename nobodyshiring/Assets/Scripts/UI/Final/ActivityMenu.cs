using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivityMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // activate statbars on hover
        Statbars.Instance.energy.ShowPreview();
        Statbars.Instance.motivation.ShowPreview();

        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Opening");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // deactivate statbars on unhover
        Statbars.Instance.energy.HidePreview();
        Statbars.Instance.motivation.HidePreview();

        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Closing");
    }
}
