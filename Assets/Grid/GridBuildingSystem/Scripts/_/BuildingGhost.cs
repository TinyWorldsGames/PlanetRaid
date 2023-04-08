using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {

    [SerializeField]
    private List<Transform> visuals = new List<Transform>();
    private PlacedObjectTypeSO.Dir dir;

    private void OnEnable()
    {
        
    }
    private void Start()
    {
        GridBuildingSystem3D.Instance.OnSelectedChanged += RefreshVisual;
        GridBuildingSystem3D.Instance.OnPathFound += CreateVisual;

    }

    private void OnDisable()
    {
        GridBuildingSystem3D.Instance.OnSelectedChanged -= RefreshVisual;
        GridBuildingSystem3D.Instance.OnPathFound -= CreateVisual;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

    }

    private void RefreshVisual()
    {

        for (int i = 0; i < visuals.Count; i++)
        {
            Destroy(visuals[i].gameObject);
        }

        visuals.Clear();
    }



    private void CreateVisual(PlacedObjectTypeSO placedObjectTypeSO,Vector3 placedObjectWorldPosition,Quaternion roation)
    {
       // RefreshVisual();
        
        if (placedObjectTypeSO != null) {
            Transform visual;
            visual = Instantiate(placedObjectTypeSO.visual, placedObjectWorldPosition, roation);
            visual.parent = transform;
            visuals.Add(visual);

        }
    }

    

}

