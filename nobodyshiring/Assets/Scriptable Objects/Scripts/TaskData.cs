using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Scriptable Objects/TaskData")]
public class TaskData : ProjectData
{
    public string taskName;
    [Space]
    public float recommendedCoding;
    public float recommendedNetworking;
    public float recommendedMarketing;
    public float recommendedGameDesign;
    public float recommendedUnity;
    public float recommendedUnrealEngine;
    public float recommendedGodot;
    public float recommendedGameMaker;
    public float recommendedAudio;
    public float recommendedArt;
    [Space]
    public float maxProgress;
    [Space]
    public float pay;
}
