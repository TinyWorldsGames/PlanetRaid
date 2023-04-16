using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject_Done : MonoBehaviour {

public PlacedObjectTypeSO objectTypeSO;
public GridObject gridObject;
private GridXZ<GridObject> grid;

[SerializeField]
GameObject build;

[SerializeField]
int state;

void OnEnable()
{
    
        GameEvents.Instance.OnControlBuildings+=Control;
        grid=GridBuildingSystem3D.Instance.grid;
        objectTypeSO=GridBuildingSystem3D.Instance.placedObjectTypeSO;
    
}

void OnDisable()
{
    if(objectTypeSO.isNeedControl)
    {
        GameEvents.Instance.OnControlBuildings+=Control;

    }
}


// Üst 1 
// Sag 2
// Alt 4
// Sol 8

// 
void Control()
{
  
    if(objectTypeSO.isNeedControl)
    {

        int newState=0;
         GridObject newGridobject=null;

      if (gridObject.x - 1 >= 0)
        {
           newGridobject=GetNode(gridObject.x - 1, gridObject.y);
            
            
            if( newGridobject.placedObject!=null &&newGridobject.placedObject.objectTypeSO.name.Equals(objectTypeSO.name))
            {
                newState+=8;
                
            }
           
           
        }

        if (gridObject.x + 1 < grid.GetWidth())
        {

            newGridobject=GetNode(gridObject.x + 1, gridObject.y);


            if( newGridobject.placedObject!=null &&newGridobject.placedObject.objectTypeSO.name.Equals(objectTypeSO.name))
            {
                newState+=2;
            }

        }
       
        if (gridObject.y - 1 >= 0) 
        {
            newGridobject=GetNode(gridObject.x  , gridObject.y-1);
           
            if( newGridobject.placedObject!=null &&newGridobject.placedObject.objectTypeSO.name.Equals(objectTypeSO.name))
            {
                newState+=4;
            }

        }
       
        if (gridObject.y + 1 < grid.GetHeight())
        {

            newGridobject=GetNode(gridObject.x  , gridObject.y+1);


            if( newGridobject.placedObject!=null &&newGridobject.placedObject.objectTypeSO.name.Equals(objectTypeSO.name))
            {
                newState+=1;
            }

        }

        if(state!=newState)
        {
            state=newState;
            GameObject newBuild= Instantiate(objectTypeSO.buildVariations[state],build.transform.position,Quaternion.identity);
            Destroy(build);
            build = newBuild;
          
        }



    }
    




}

   GridObject GetNode(int x, int y)
    {
        GridObject gridObject =grid.GetGridObject(x, y);
      
        return gridObject;
    }

    public static PlacedObject_Done Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject_Done placedObject = placedObjectTransform.GetComponent<PlacedObject_Done>();
        placedObject.Setup(placedObjectTypeSO, origin, dir);
        
        

        return placedObject;
    }

    public static Transform  CreateVisual(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.visual, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

       
        return placedObjectTransform;
    }



    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin, PlacedObjectTypeSO.Dir dir) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
        this.dir = dir;
    }

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public override string ToString() {
        return placedObjectTypeSO.nameString;
    }

}
