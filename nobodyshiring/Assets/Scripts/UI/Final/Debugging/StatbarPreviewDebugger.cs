using UnityEngine;

public class StatbarPreviewDebugger : MonoBehaviour
{

    [SerializeField] Statbars statbars;

    public void ShowMotivationPreview()
    {
        statbars.ShowPreview(PlayerStat.motivation, 80);
    }

    public void ShowEnergyPreview()
    {
        statbars.ShowPreview(PlayerStat.energy, 80);
    }

    public void HideEnergyPreview()
    {
        statbars.HidePreview(PlayerStat.energy);
    }

    public void HideMotivationPreview()
    {
        statbars.HidePreview(PlayerStat.motivation);
    }


    public void ApplyEnergyPreview()
    {
        PlayerStats.Instance.SetEnergy(80);
    }
}
