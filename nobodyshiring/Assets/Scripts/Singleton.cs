using UnityEngine;

public class Singleton : MonoBehaviour
{

    public static Singleton Instance;

    protected virtual void Awake()
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
