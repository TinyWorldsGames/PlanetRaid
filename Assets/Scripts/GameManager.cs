using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

 


   [SerializeField]
   List<Resource> resources;

   void Awake()
   {

    resources.Add(new Resource());
   }

 
}
