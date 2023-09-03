using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSetter : MonoBehaviour
{

    GridBuildingSystem3D gridBuldingSystem;
    public GridXZ<GridObject> grid;

    [SerializeField] PlacedObjectTypeSO placedObjectTypeSO;

    private void Start()
    {
        gridBuldingSystem = GridBuildingSystem3D.Instance;
        grid = gridBuldingSystem.grid;

        BuildObject();

    }

    void BuildObject()
    {
        gridBuldingSystem.placedObjectTypeSO = placedObjectTypeSO;
        grid.GetXZ(transform.position, out int x, out int z);

        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

        // Test Can Build
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, PlacedObjectTypeSO.Dir.Up);
        bool canBuild = true;

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            GridObject gridObject1 = grid.GetGridObject(gridPosition.x, gridPosition.y);

            if ((gridObject1.CanBuild() == 0) || (gridObject1.CanBuild() == 1 && (placedObjectTypeSO.isUnderground)) || (gridObject1.CanBuild() == 2 && (!placedObjectTypeSO.isUnderground)))
            {

            }
            else
            {

                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
           
            GridObject gridObject = grid.GetGridObject(x, z);
            GameEvents.Instance.OnGridSelected?.Invoke(placedObjectTypeSO,gridObject);

            //DeselectObjectType();
        }
       

    }

}
