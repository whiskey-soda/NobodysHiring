using UnityEngine;

public class Statbars : MonoBehaviour
{

    [SerializeField] public StatbarController energy;
    [SerializeField] public StatbarController motivation;

    public static Statbars Instance;

    private void Awake()
    {
        // singleton code
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
}
