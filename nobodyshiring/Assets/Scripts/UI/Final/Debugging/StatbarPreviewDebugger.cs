using UnityEngine;

public class StatbarPreviewDebugger : MonoBehaviour
{
    [SerializeField] StatbarController energyStatbar;
    [SerializeField] StatbarController motivationStatbar;
    [Space]
    [SerializeField] float previewValue = 80;

    public void ShowMotivationPreview()
    {
        motivationStatbar.ShowPreview(previewValue);
    }

    public void ShowEnergyPreview()
    {
        energyStatbar.ShowPreview(previewValue);
    }

    public void HideEnergyPreview()
    {
        energyStatbar.HidePreview();
    }

    public void HideMotivationPreview()
    {
        motivationStatbar.HidePreview();
    }


    public void ApplyEnergyPreview()
    {
        PlayerStats.Instance.SetEnergy(previewValue);
    }
}
