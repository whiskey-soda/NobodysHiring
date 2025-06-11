using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivityMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Opening");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Closing");
    }
}
