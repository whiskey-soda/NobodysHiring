using UnityEngine;

public class StatbarPreviewDebugger : MonoBehaviour
{

    [SerializeField] Statbars statbars;

    public void ShowMotivationPreview()
    {
        statbars.ShowPreview(PlayerStat.motivation, 40);
    }

    public void ShowEnergyPreview()
    {
        statbars.ShowPreview(PlayerStat.energy, 40);
    }

    public void HidePreviews()
    {
        statbars.HidePreviews();
    }
}
