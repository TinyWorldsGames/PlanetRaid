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

    [SerializeField]
    Enums.ResourceTypes resourceTypes;

    private void Start()
    {

        playerTransform = PlayerController.Instance.transform;

    }

    public void Collect()
    {
        GameObject newCollectable = Instantiate(collectable, spawnPoint.position, Quaternion.identity);

        StartCoroutine(FollowTheTarget(newCollectable));



    }

    IEnumerator FollowTheTarget(GameObject newCollectable)
    {
        while (true)
        {
            newCollectable.transform.position = Vector3.Lerp(newCollectable.transform.position, playerTransform.position, 0.1f);

            if (Vector3.Distance(newCollectable.transform.position, playerTransform.position) < 0.5f)
            {
                GameEvents.Instance.OnResourceCollacted?.Invoke(resourceTypes);

                if (resourceTypes == Enums.ResourceTypes.Odun)
                {


                    TaskController.Instance.Task1Update(resourceTypes);



                }

                else
                {
                    Debug.Log("Task 2 Updated");
                    TaskController.Instance.Task2Update(resourceTypes);
                }

                break;




            }
            yield return null;
        }

        Destroy(newCollectable);
    }
}
