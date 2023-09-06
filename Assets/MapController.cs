using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private IslandType[] islandTypes;

    [SerializeField]
    private List<Island> islands = new List<Island>();

    [SerializeField]
    private SpriteRenderer islandAreaSpritePrefab;

    private int mapWidth = 500;
    private int mapHeight = 500;

    Vector2 startPoint = new Vector2(235, 235);
    Vector2 endPoint = new Vector2(265, 265);

    int islandSizeX, islandSizeY;

    private void Start()
    {
        CreateRandomIslands();
    }



    void CreateRandomIslands()
    {
      
        Vector2 _previousEndPoint = Vector2.zero;

        for (int j = 0; j < mapHeight; j += 30)
        {
            for (int i = 0; i < mapWidth; i += 30)
            {
                Vector2 _startPoint = new Vector2(i, j);

                Vector2 _endPoint = _startPoint + new Vector2(30, 30);

                if (_endPoint.x > mapWidth || _endPoint.y > mapHeight)
                {
                    break;
                }

                int randomType = Random.Range(0, islandTypes.Length);
                 CreateIsland(_startPoint, _endPoint, islandTypes[randomType]);
                _previousEndPoint = _endPoint;
            }

            if (_previousEndPoint.y > mapHeight)
            {
                break;
            }
        }
    }

    Island CreateIsland(Vector2 startPoint, Vector2 endPoint, IslandType type)
    {
        Island island = new Island(type, startPoint, endPoint);
        SpriteRenderer islandAreaSprite = Instantiate(islandAreaSpritePrefab);
        islandAreaSprite.transform.localScale = new Vector3(island.endPosition.x - island.startPosition.x, island.endPosition.y - island.startPosition.y, 1);
        islandAreaSprite.transform.position = new Vector3((island.startPosition.x + island.endPosition.x) / 2, 0, (island.startPosition.y + island.endPosition.y) / 2);
        islandAreaSprite.color = island.type.color;
        islands.Add(island);
        return island;

    }

}
