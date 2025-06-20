using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]

public class Btn_HoverOutline : HoverOutline
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        // if outline is active and button is not interactable, hide the outline
        // prevents outline being erroneously shown on buttons that can turn non-interactable while hovered
        if (!button.interactable && panelOutline.color.a != 0) { HideOutline(); }
    }

    protected override void ShowOutline()
    {
        // only show outline if button is interactable
        if (button.interactable) { base.ShowOutline(); }
    }
}
