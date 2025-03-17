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
        statsText.text = $"stats\n" +
            $"motivation: {playerStats.motivation}\n" +
            $"energy:  {playerStats.energy}\n";
    }
}
