using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GigData", menuName = "Scriptable Objects/GigData")]
public class GigData : ProjectData
{
    public string gigName;
    public List<TaskData> stages = new List<TaskData>();
}
