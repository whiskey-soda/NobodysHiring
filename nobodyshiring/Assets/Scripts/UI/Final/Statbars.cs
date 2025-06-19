using UnityEngine;

public class Statbars : MonoBehaviour
{

    [SerializeField] StatbarController energy;
    [SerializeField] StatbarController motivation;

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
