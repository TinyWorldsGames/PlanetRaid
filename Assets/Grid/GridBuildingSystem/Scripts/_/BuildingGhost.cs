using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private List<Transform> visuals = new List<Transform>();

    [SerializeField]
    Material cantBuildMaterial;

    [SerializeField]
    Transform visualsParent;

    GridObject _currentGridObject;
    GridObject _gridObject;

    GridBuildingSystem3D gridBuildingSystem3D;
    GridXZ<GridObject> grid;

    Vector3 targetPosition;

    Material[][] defaultMaterials;
    Material[] _cantBuildMaterials;

    private void Start()
    {
        gridBuildingSystem3D = GridBuildingSystem3D.Instance;
        grid = gridBuildingSystem3D.grid;


        GameEvents.Instance.OnSelectedChanged += RefreshVisual;
        GameEvents.Instance.OnSelectedChangedStackable += ClearVisuals;
        GameEvents.Instance.OnPathFound += CreateVisual;

    }

    private void OnDisable()
    {
        GameEvents.Instance.OnSelectedChanged -= RefreshVisual;
        GameEvents.Instance.OnSelectedChangedStackable -= ClearVisuals;
        GameEvents.Instance.OnPathFound -= CreateVisual;

    }

    private void Update()
    {
        targetPosition = GridBuildingSystem3D.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem3D.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);

        if (visuals.Count > 0)
        {
            _currentGridObject = grid.GetGridObject(targetPosition);
            if (_currentGridObject != _gridObject)
            {
                _gridObject = _currentGridObject;
                ChangeMaterial(_gridObject);

            }

        }


    }

    void ChangeMaterial(GridObject gridObject)
    {


        for (int i = 0; i < _cantBuildMaterials.Length; i++)
        {
            _cantBuildMaterials[i] = cantBuildMaterial;
        }

        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO();
        if ((gridObject.CanBuild() == 0) || (gridObject.CanBuild() == 1 && (placedObjectTypeSO.isUnderground)) || (gridObject.CanBuild() == 2 && (!placedObjectTypeSO.isUnderground)))
        {
            MeshRenderer[] visualsMaterials = visuals[0].GetComponentsInChildren<MeshRenderer>();

            visualsMaterials[0].materials = _cantBuildMaterials;

        }
        else
        {
            MeshRenderer[] visualsMaterials = visuals[0].GetComponentsInChildren<MeshRenderer>();

            visualsMaterials[0].materials = defaultMaterials[0];
        }
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


            defaultMaterials = new Material[visuals[0].GetComponentInChildren<MeshRenderer>().materials.Length][];
            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                defaultMaterials[i] = visuals[0].GetComponentInChildren<MeshRenderer>().materials;
            }

            _cantBuildMaterials = new Material[defaultMaterials.Length];



            for (int i = 0; i < _cantBuildMaterials.Length; i++)
            {
                _cantBuildMaterials[i] = cantBuildMaterial;
            }


        }
    }



    private void CreateVisual(GridObject gridObject)
    {
        // RefreshVisual();
        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO();
        if (placedObjectTypeSO != null)
        {

            GridXZ<GridObject> grid = GridBuildingSystem3D.Instance.grid;
            Vector2Int placedObjectOrigin = new Vector2Int(gridObject.x, gridObject.y);

            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(gridObject.dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            Transform visual = PlacedObject_Done.CreateVisual(placedObjectWorldPosition, placedObjectOrigin, gridObject.dir, placedObjectTypeSO);
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, gridObject.dir);

            if ((gridObject.CanBuild() == 0) || (gridObject.CanBuild() == 1 && (placedObjectTypeSO.isUnderground)) || (gridObject.CanBuild() == 2 && (!placedObjectTypeSO.isUnderground)))
            {

            }
            else
            {
                MeshRenderer[] visualsMaterials = visual.GetComponentsInChildren<MeshRenderer>();

                for (int i = 0; i < visualsMaterials.Length; i++)
                {

                    visualsMaterials[i].materials = _cantBuildMaterials;
                }
            }
            visuals.Add(visual.transform);

        }
    }



}

