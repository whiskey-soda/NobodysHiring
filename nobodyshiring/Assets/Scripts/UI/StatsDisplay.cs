using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StatsDisplay : MonoBehaviour
{

    PlayerStats playerStats;

    TextMeshProUGUI statsText;

    private void Awake()
    {
        statsText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        statsText.text = $"motivation: {System.Math.Round(playerStats.motivation)}\n" +
            $"energy:  {System.Math.Round(playerStats.energy)}\n";
    }
}
