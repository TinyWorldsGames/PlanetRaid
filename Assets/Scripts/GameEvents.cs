using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    // Delegates;
    public delegate void OnSelectedChangedDelegate();
    public delegate void GridSelectedDelagate(PlacedObjectTypeSO objectTypeSO,GridObject gridObject);
    public delegate void PathFoundDelagate(GridObject gridObject);
    public delegate void ObjectPlacedDelegate(GridObject objectTypeSO);
    public delegate void ResourceChangedDelegate(Enums.ResourceTypes resourceType,int count);
   // public delegate void ControlBuildings();

    
    // Delegate Instance
    public GridSelectedDelagate OnGridSelected;
    public PathFoundDelagate OnPathFound;
    public ObjectPlacedDelegate OnObjectPlaced;
   // public ControlBuildings OnControlBuildings;
    public OnSelectedChangedDelegate OnSelectedChangedStackable;
    public OnSelectedChangedDelegate OnSelectedChanged;
    public ResourceChangedDelegate OnResourceChanged;

    

    
    void Awake()
    {
        Instance=this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
                OnResourceChanged?.Invoke(Enums.ResourceTypes.Iron,1);
        }
    }



}
