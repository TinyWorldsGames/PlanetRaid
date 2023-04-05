using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private GridXZ<GridObject> grid;
    [SerializeField]
    private List<GridObject> openList;
    [SerializeField]
    private List<GridObject> closedList;

    [SerializeField]
    List<GridObject> path = new List<GridObject>();

   
    GridObject startNode, endNode,selectedNode;
  

    int safer;
    bool secondGridSelected;
    bool isWorking;
    private void Start()
    {
        grid = GridBuildingSystem3D.Instance.grid;

    }

    private void OnEnable()
    {
        GridBuildingSystem3D.Instance.OnGridSelected += CreateNewBuilding;
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
      
    }


    IEnumerator CreatingStackableBuild()
    {
        startNode = null;
        selectedNode = null;
        List<GridObject> finalPath=null;
        secondGridSelected = true;
        isWorking = true;

        while (selectedNode==null&&safer<5000)
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            safer++;

            if (grid.GetGridObject(mousePosition) != null)
            {
                grid.GetXZ(mousePosition, out int x, out int z);
                
                if (Input.GetMouseButtonDown(0))
                {
                    
                    if (startNode == null)
                    {
                        startNode = grid.gridArray[x, z];
                        endNode = grid.gridArray[x, z];
                    }

                   
                }

                endNode = grid.gridArray[x, z];
                finalPath = FindPath(startNode, endNode);


            }

            else
            {
                finalPath = null;
            }
            


           

            if (finalPath!=null&&!secondGridSelected)
            {
                Debug.Log("Final Path Bulundu");

                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < finalPath.Count; i++)
                    {
                        selectedNode = endNode;
                        secondGridSelected = true;
                        GridBuildingSystem3D.Instance.OnObjectPlaced(finalPath[i]);
                        Debug.Log("SAG TIK BASILDI"+"Final Path Count" + finalPath.Count);
                    }

                  


                }


            }

            yield return new WaitForSeconds(0);
           
            secondGridSelected = false;

        }

        isWorking = false;


    }




    public void CreateNewBuilding(PlacedObjectTypeSO objectTypeSO)
    {
        if (objectTypeSO.isStackable&&!isWorking)
        {
            isWorking = true;
            StartCoroutine(CreatingStackableBuild());
        }

        return;
    }



    //public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    //{
    //    grid.GetXZ(startWorldPosition, out int startX, out int startY);
    //    grid.GetXZ(endWorldPosition, out int endX, out int endY);


    //     path = FindPath(startX, startY, endX, endY);
    //    if (path == null)
    //    {
    //        return null;
    //    }
    //    else
    //    {
    //        List<Vector3> vectorPath = new List<Vector3>();
    //        foreach (GridObject pathNode in path)
    //        {
    //            vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
    //        }
    //        return vectorPath;
    //    }
    //}

    public List<GridObject> FindPath(GridObject startNode, GridObject endNode)
    {


    

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<GridObject> { startNode };
        closedList = new List<GridObject>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                GridObject pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

       
        while (openList.Count > 0)
        {
            GridObject currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
             
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridObject neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.CanBuild())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                neighbourNode.cameFromNode = currentNode;
                int turnCost = CalculateDirectionCost(currentNode, neighbourNode);

               
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode) + turnCost;
                if (tentativeGCost < neighbourNode.gCost)
                {
                   
                    neighbourNode.gCost = tentativeGCost - turnCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
               
            }
        }

        // Out of nodes on the openList
        return null;
    }

    int CalculateDirectionCost(GridObject current, GridObject next)
    {
        // Up 1
        // Down 2
        // Left 3
        // Righ 4


        if (current.x > next.x)
        {
            next.comeDirection = 4;
        }

        else if (current.x < next.x)
        {
       
            next.comeDirection = 3;
        }

        else if (current.y > next.y)
        {
            next.comeDirection = 1;
        }

        else if (current.y < next.y)
        {
          
            next.comeDirection = 2;

        }


        if (current.comeDirection != next.comeDirection && current.comeDirection != 0)
        {

            next.turnCost = 1;
            return 1;
        }
        next.turnCost = 0;
        return 0;
    }
    private List<GridObject> GetNeighbourList(GridObject currentNode)
    {
        List<GridObject> neighbourList = new List<GridObject>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //// Left Down
            //if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //// Left Up
            //if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

            // Right Down
            //if (currentNode.y - 1 >= 0) 
            //    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            //// Right Up
            //if (currentNode.y + 1 < grid.GetHeight()) 
            //    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public GridObject GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<GridObject> CalculatePath(GridObject endNode)
    {
        for (int i = 0; i < path.Count; i++)
        {
            grid.debugTextArray[path[i].x, path[i].y].text = path[i].x + " " + path[i].y;
        }

        path.Clear();
        path.Add(endNode);
        grid.debugTextArray[endNode.x, endNode.y].text = endNode.x + " " + endNode.y + "\n Furkan burda";

        GridObject currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            grid.debugTextArray[currentNode.cameFromNode.x, currentNode.cameFromNode.y].text = currentNode.cameFromNode.x+" "+currentNode.cameFromNode.y+"\n Furkan burda";
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(GridObject a, GridObject b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        //return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        return xDistance + yDistance;
    }

    private GridObject GetLowestFCostNode(List<GridObject> pathNodeList)
    {
        GridObject lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}