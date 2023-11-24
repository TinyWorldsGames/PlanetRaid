using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Transform endPosition;

    Vector3 startPoint;

    [SerializeField]
    GameObject door;


  


    Tween tween;

    bool isTestAnimStarted = false;

    private void Start()
    {
        startPoint = door.transform.position;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {


            tween.Kill();

            audioSource.Play();

            tween = door.transform.DOMove(endPosition.position, 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            tween.Kill();

            audioSource.Play();
            
            tween = door.transform.DOMove(startPoint, 2f);
        }
    }
}
