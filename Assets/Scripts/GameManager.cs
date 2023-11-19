using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   public BuildResources buildResources;


   [SerializeField]
   List<Resource> resources;

   void Awake()
   {
      resources.Add(new Resource());
   }

   private void OnEnable()
   {
      GameEvents.Instance.OnResourceControl += ControlResource;
   }

   bool ControlResource(BuildResources buildResources)
   {
      if (buildResources.resource1 <= this.buildResources.resource1 && buildResources.resource2 <= this.buildResources.resource2)
      {
         return true;
      }

      else
      {
         return false;
      }


   }


}
