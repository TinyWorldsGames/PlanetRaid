using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   public BuildResources buildResources;

   public int batteryCount;
   public int oxygenGeneratorCount;

   public int solarPanelCount;
   public int farmCount;

   public int currentWater = 100, currentOxygen = 100, currenStorage = 100, currentHealth = 100, currentScience = 0;
   public float currentBattery = 100;
   public float batterCapacity = 100;

   int ironMinerCounr;
   int copperMinerCount;

   public int deathEnemyCount;

   public int electicityUsage = 0;

   [SerializeField]
   Image storageImage, oxygenImage;


   [SerializeField]
   Image electricArrow, ironArrow, copperArrow, electricityBar;


   [SerializeField] TMP_Text totalElectricityChange, woodCountText, waterCountText, oxygenCountText, electricityCountText, storageCountText, healthCountText, scienceCountText, ironCountText, carbonCountText;



   void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
      {
         Destroy(gameObject);
      }


   }



   private void OnEnable()
   {
      GameEvents.Instance.OnResourceControl += ControlResource;
      GameEvents.Instance.OnResourceUsed += UseResource;
      GameEvents.Instance.OnResourceCollacted += ResourcesCollected;
   }

   private void OnDisable()
   {
      GameEvents.Instance.OnResourceControl -= ControlResource;
      GameEvents.Instance.OnResourceUsed -= UseResource;
      GameEvents.Instance.OnResourceCollacted -= ResourcesCollected;

   }



   void ResourcesCollected(Enums.ResourceTypes resourceTypes)
   {
      switch (resourceTypes)
      {
         case Enums.ResourceTypes.Demir:
            buildResources.resource1 += 10;
            break;
         case Enums.ResourceTypes.Bakir:
            buildResources.resource2 += 10;
            break;
         case Enums.ResourceTypes.Odun:
            buildResources.resource3 += 10;
            break;
         default:
            break;
      }
   }


   private void Start()
   {
      StartCoroutine(WaterRoutine());
      StartCoroutine(OxygenRoutine());
      StartCoroutine(EnergyRoutine());
      StartCoroutine(ScineRoutine());
      StartCoroutine(ResourceRoutine());


   }

   public void OpenMenu()
   {
      SceneManager.LoadScene(0);
   }

   public void AddMiner(Enums.ResourceTypes resourceType)
   {
      if (resourceType == Enums.ResourceTypes.Demir)
      {
         ironMinerCounr++;
      }

      else if (resourceType == Enums.ResourceTypes.Bakir)
      {
         copperMinerCount++;
      }



   }


   IEnumerator ResourceRoutine()
   {
      while (true)
      {
         yield return new WaitForSeconds(2);



         buildResources.resource1 += ironMinerCounr;
         buildResources.resource2 += copperMinerCount;

         ironCountText.text = buildResources.resource1.ToString();
         carbonCountText.text = buildResources.resource2.ToString();


      }
   }

   IEnumerator EnergyRoutine()
   {
      while (true)
      {
         yield return new WaitForSeconds(1);

         int woodEnergy = buildResources.resource3 * 2;

         int increaseAmount = woodEnergy + (solarPanelCount * 6);

         currentBattery += increaseAmount;

         currentBattery -= (int)electicityUsage / 10;

         int totalChange = increaseAmount - (int)electicityUsage / 10;



         if (currentBattery < 0)
         {
            currentBattery = 0;
         }

         batterCapacity = 100 + (batteryCount * 125);

         if (currentBattery > batterCapacity)
         {
            currentBattery = batterCapacity;
         }

         if (totalChange > 0)
         {
            electricArrow.color = Color.green;
            electricArrow.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 90), 0.125f);

         }
         else
         {
            electricArrow.color = Color.red;
            electricArrow.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 270), 0.125f);
         }

         totalElectricityChange.text = totalChange.ToString();

         woodCountText.text = buildResources.resource3.ToString();

         electricityCountText.text = currentBattery.ToString() + "/" + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.blue)}>" + increaseAmount + "</color>" + batterCapacity.ToString();

         electricityBar.fillAmount = currentBattery / batterCapacity;


      }
   }

   IEnumerator OxygenRoutine()
   {
      while (true)
      {
         yield return new WaitForSeconds(5);

         int increaseAmount = oxygenGeneratorCount * 30;

         currentOxygen += increaseAmount;

         currentOxygen -= 1;

         oxygenImage.fillAmount = currentOxygen / 100f;



      }
   }

   IEnumerator WaterRoutine()
   {
      while (true)
      {
         yield return new WaitForSeconds(6);

         currentWater -= 1;

         waterCountText.text = currentWater.ToString() + "/100";
      }
   }

   IEnumerator ScineRoutine()
   {
      while (true)
      {
         yield return new WaitForSeconds(5);

         currentScience += deathEnemyCount / 5;

         deathEnemyCount = 0;

         scienceCountText.text = currentScience.ToString();
      }

   }








   void UseResource(BuildResources buildResources)
   {
      this.buildResources.resource1 -= buildResources.resource1;
      this.buildResources.resource2 -= buildResources.resource2;

      ironCountText.text = this.buildResources.resource1.ToString();
      carbonCountText.text = this.buildResources.resource2.ToString();
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
