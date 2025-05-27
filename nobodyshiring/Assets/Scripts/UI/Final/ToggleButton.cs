using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    bool on = false;

    public UnityEvent onEvent;
    public UnityEvent offEvent;

    public void Toggle()
    {
        if (!on) { onEvent.Invoke(); on = true; }
        else if (on) {  offEvent.Invoke(); on = false; }
    }
}
