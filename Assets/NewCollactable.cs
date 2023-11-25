using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewCollactable : MonoBehaviour, ICollectableNew
{
    [SerializeField]
    GameObject collectable;

    [SerializeField]
    Transform spawnPoint;

    Transform playerTransform;

    private void Start()
    {

        playerTransform = PlayerController.Instance.transform;
        
    }

    public void Collect()
    {
        GameObject newCollectable = Instantiate(collectable, spawnPoint.position, Quaternion.identity);

        newCollectable.transform.DOMove(playerTransform.position, 0.35f).OnComplete(() =>
        {
            Destroy(newCollectable);
        });



    }
}
