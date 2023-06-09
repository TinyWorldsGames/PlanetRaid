using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource 
{
    public string name;
    public int count;
    public Enums.ResourceTypes resourceType = Enums.ResourceTypes.Iron;

    public Resource()
    {
        name = resourceType.ToString();
    }
}
