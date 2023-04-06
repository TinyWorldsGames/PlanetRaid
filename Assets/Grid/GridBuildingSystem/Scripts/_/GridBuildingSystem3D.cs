using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridBuildingSystem3D : MonoBehaviour {

    public static GridBuildingSystem3D Instance { get; private set; }

    public delegate void OnSelectedChangedDelegate();
    public delegate void GridSelectedDelagate(PlacedObjectTypeSO objectTypeSO);
    public delegate void PathFoundDelagate(PlacedObjectTypeSO objectTypeSO,Vector3 position , Quaternion quaternion);
    public delegate void ObjectPlacedDelegate(GridObject objectTypeSO);
    public GridSelectedDelagate OnGridSelected;
    public PathFoundDelagate OnPathFound;
    public ObjectPlacedDelegate OnObjectPlaced;
    public OnSelectedChangedDelegate OnSelectedChanged;


    public GridXZ<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    [SerializeField]
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;
    private GridObject startNode, endNode;
    [SerializeField]
    Pathfinding pathfinding;
    

    private void Awake() {
        Instance = this;

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedObjectTypeSO = null;// placedObjectTypeSOList[0];
    }

    private void OnEnable()
    {
        OnObjectPlaced += CreateNewBuilding;
    }
    private void OnDisable()
    {
         OnObjectPlaced -= CreateNewBuilding;

    }


    public void CreateNewBuilding(GridObject gridObject)
    {
      

        Vector2Int placedObjectOrigin = new Vector2Int(gridObject.x, gridObject.y);
        //  placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
        Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
        Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
        PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);


        foreach (Vector2Int gridPosition in gridPositionList)
        {
            grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
        }

    }



    private void Update() 
    {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null) 
        {
            
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild) 
            {
               
                OnGridSelected?.Invoke(placedObjectTypeSO);

                //DeselectObjectType();
            } 

            else 
            {
                // Cannot build here
                Utils.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.E)) { placedObjectTypeSO = null; RefreshSelectedObjectType(); }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }

        if (Input.GetKeyDown(KeyCode.Alpha7)) 
        {
            Debug.Log(grid.gridArray[3, 2].CanBuild());


        }


        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        //  //  grid.GetXZ(mousePosition, out int x, out int y);
        //    //List<GridObject> path = pathfinding.FindPath(0, 0, x, y);


        //    if (grid.GetGridObject(mousePosition) != null)
        //    {
        //        grid.GetXZ(mousePosition, out int x, out int z);

        //        if (startNode == null)
        //        {
        //            startNode = grid.gridArray[x, z];
        //            endNode = grid.gridArray[x, z];
                    
        //        }

        //        endNode = grid.gridArray[x, z];

        //        pathfinding.FindPath(startNode,endNode);

        //    }

        //    else
        //    {
        //        startNode = null;

        //    }

            
        //}

        //else
        //{
        //    startNode=null;
        //}





       
    }

    public PlacedObjectTypeSO.Dir GetDirection()
    {
        return dir;
    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke();
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

}

[System.Serializable]
public class GridObject
{
    private GridXZ<GridObject> grid;
    public int x;
    public int y;
    public PlacedObject_Done placedObject;

    public int gCost;
    public int hCost;
    public int fCost;
    public int comeDirection;
    public int turnCost;
    public GridObject cameFromNode;





    public GridObject(GridXZ<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        placedObject = null;
      
    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + y + "\n" + placedObject;
    }

    public void SetPlacedObject(PlacedObject_Done placedObject)
    {
        this.placedObject = placedObject;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
        grid.TriggerGridObjectChanged(x, y);
    }

    public PlacedObject_Done GetPlacedObject()
    {
        return placedObject;
    }

    public bool CanBuild()
    {
        return placedObject == null;
    }

}

//public class Node
//{
//    public int x, z, gCost, hCost,fCost;
//    public PlacedObjectTypeSO.Dir dir;
//    public Node priviusNode;
//    public bool canBuild;

//    public Node(int x, int z)
//    {
//        this.x = x;
//        this.z = z;
//    }
//}
