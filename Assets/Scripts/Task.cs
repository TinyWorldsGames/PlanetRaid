
using UnityEngine;


[CreateAssetMenu(fileName = "Task", menuName = "New Task", order = 1)]
public class Task : ScriptableObject
{
    public string taskName;
    public string taskDescription;
    public string taskDescription2;
    
    public string taskEnd;
    

    public bool isCompleted;

}