using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image panelOutline;

    /// <summary>
    /// makes panel outline color transparent
    /// </summary>
    protected virtual void ShowOutline()
    {
        Color color = panelOutline.color;
        color.a = 1;
        panelOutline.color = color;
    }

    /// <summary>
    /// makes panel outline color opaque
    /// </summary>
    protected void HideOutline()
    {
        Color color = panelOutline.color;
        color.a = 0;
        panelOutline.color = color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        ShowOutline();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        HideOutline();
    }
}
