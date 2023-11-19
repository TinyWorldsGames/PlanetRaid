using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PortableMiner : MonoBehaviour
{
    [SerializeField]
    int capacity;
    [SerializeField]
    int currentResource;
    [SerializeField]
    Enums.ResourceTypes resourceType;

    [SerializeField]
    Text capacityText;

    bool _isWorking;

    
   
}
