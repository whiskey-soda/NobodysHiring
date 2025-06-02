using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]

public class ProgressBarMask : MonoBehaviour
{
    [SerializeField] float barWidth = 65;
    RectMask2D mask;

    private void Awake()
    {
        mask = GetComponent<RectMask2D>();
    }

    /// <summary>
    /// updates the "right" padding value of a rect mask to match the current progress of a given stat value,
    /// based on the stat's current and maximum values.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    protected void UpdateMaskRect(float current, float max)
    {
        // calculate width of the padding to match proportion
        float progressProportion = current / max;
        float paddingWidth = barWidth - (barWidth * progressProportion);

        // apply (x=left, y=bottom, **z=right**, w=top)
        mask.padding = new Vector4(mask.padding.x, mask.padding.y, paddingWidth, mask.padding.w);
    }
}
