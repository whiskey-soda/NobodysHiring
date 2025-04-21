using UnityEngine;

public class PersonalProjectAdder : MonoBehaviour
{

    [SerializeField] TaskData personalProjectData;
    [SerializeField] WorkManager workManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        workManager = WorkManager.Instance;
        AddPersonalProject();
    }

    void AddPersonalProject()
    {
        workManager.AddWork(new Job(personalProjectData));
    }
}
