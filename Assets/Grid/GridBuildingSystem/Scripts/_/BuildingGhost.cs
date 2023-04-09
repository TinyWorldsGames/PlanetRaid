using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    private List<Transform> visuals = new List<Transform>();
    
    [SerializeField]
    Material canBuildMaterial,cantBuildMaterial;

    [SerializeField]
    Transform visualsParent;
    
    private void OnEnable()
    {
        
    }

    private void Start()
    {
        GridBuildingSystem3D.Instance.OnSelectedChanged += RefreshVisual;
        GridBuildingSystem3D.Instance.OnSelectedChangedStackable += ClearVisuals;

        GridBuildingSystem3D.Instance.OnPathFound += CreateVisual;

    }

    private void OnDisable()
    {
        GridBuildingSystem3D.Instance.OnSelectedChanged -= RefreshVisual;
        GridBuildingSystem3D.Instance.OnSelectedChangedStackable -= ClearVisuals;
        GridBuildingSystem3D.Instance.OnPathFound -= CreateVisual;

    }

    private void Update()
    {
        Vector3 targetPosition = GridBuildingSystem3D.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem3D.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);

    }

    private void ClearVisuals()
    {
         for (int i = 0; i < visuals.Count; i++)
        {
            Destroy(visuals[i].gameObject);
        }

        visuals.Clear();
    }

    private void RefreshVisual()
    {

        for (int i = 0; i < visuals.Count; i++)
        {
            Destroy(visuals[i].gameObject);
        }

        visuals.Clear();

         PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null) 
        {
        
            Transform visual;
            visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            visuals.Add(visual);
        }
    }



    private void CreateVisual(GridObject gridObject)
    {
       // RefreshVisual();
        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO();
        if (placedObjectTypeSO != null) {
            GridXZ<GridObject> grid = GridBuildingSystem3D.Instance.grid;
            Vector2Int placedObjectOrigin = new Vector2Int(gridObject.x, gridObject.y);
            //  placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(gridObject.dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            Transform visual = PlacedObject_Done.CreateVisual(placedObjectWorldPosition, placedObjectOrigin, gridObject.dir, placedObjectTypeSO);
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, gridObject.dir);

            if(!gridObject.CanBuild())
            {
                MeshRenderer[] visualsMaterials = visual.GetComponentsInChildren<MeshRenderer>();

                for (int i = 0; i <visualsMaterials.Length; i++)
                {
                      visualsMaterials[i].material=cantBuildMaterial;
                }
            }
            visuals.Add(visual.transform);

        }
    }

    

}

