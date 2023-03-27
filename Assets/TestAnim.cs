using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
   
    void LateUpdate()
    {
        
        transform.rotation = playerController.characterSpineRotation;


    }
}
