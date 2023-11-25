using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TaskController : MonoBehaviour
{
    [SerializeField]
    TMP_Text taskName, taskDescription;

    [SerializeField]
    Image taskImage;

    [SerializeField]
    List<Task> tasks;

    public int ironMineCount, copperMineCount, woodCutterCount;

    bool isIronMineCompleted, isCopperMineCompleted;

    int taskIndex = 0;

    bool[] istasksCompleted = new bool[6];

    public static TaskController Instance;

    private void Awake()
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

    private void Start()
    {
        taskName.text = tasks[0].taskName;

        taskDescription.text = tasks[0].taskDescription;

    }

    //Gezegene hoş geldin, Jeneratörün çalışması için hızlıca odun toplamaya başla. Sağ tıklayarak hedef alıp Sol tıklayarak ateş edebilirsin. 10 Adet Odun (green) [0/10]

    public void Task1Update(Enums.ResourceTypes resourceTypes)
    {
        if (istasksCompleted[0])
        {
            return;
        }

        if (resourceTypes == Enums.ResourceTypes.Odun)
        {
            woodCutterCount++;
        }

        taskDescription.text = "Gezegene hoş geldin, Jeneratörün çalışması için hızlıca odun toplamaya başla. Sağ tıklayarak hedef alıp Sol tıklayarak ateş edebilirsin. 10 Adet Odun " + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + woodCutterCount + "/10 ] " + "</color>";



        if (woodCutterCount >= 10)
        {
            tasks[0].isCompleted = true;
            istasksCompleted[0] = true;
            StartCoroutine(TaskComplatedAnim());
        }

    }


    //"Etrafını keşfet , Demir ve Bakır madenlerini bul, Ateş ederek madenleri topla. 10 Adet Demir [" + ironMineCount + "] 10 Adet Bakır [" + copperMineCount + "]";
    public void Task2Update(Enums.ResourceTypes resourceTypes)
    {
        Debug.Log("Task 2 Updated - 2");

        if (istasksCompleted[1] || !istasksCompleted[0])
        {
            return;
        }


        if (resourceTypes == Enums.ResourceTypes.Odun)
        {
            woodCutterCount++;
        }
        else if (resourceTypes == Enums.ResourceTypes.Demir)
        {
            ironMineCount++;
            if (ironMineCount >= 10)
            {
                ironMineCount = 10;
            }
        }
        else if (resourceTypes == Enums.ResourceTypes.Bakir)
        {
            copperMineCount++;

            if (copperMineCount >= 10)
            {
                copperMineCount = 10;
            }
        }


        taskDescription.text = "Etrafını keşfet , Demir ve Bakır madenlerini bul, Ateş ederek madenleri topla. 10 Adet Demir " + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + ironMineCount + "/10 ] " + "</color>" + " 10 Adet Bakır " + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + copperMineCount + "/10 ] " + "</color>";

        if (ironMineCount >= 10 && copperMineCount >= 10)
        {
            tasks[1].isCompleted = true;
            istasksCompleted[1] = true;
            StartCoroutine(TaskComplatedAnim());
        }

    }

    IEnumerator TaskComplatedAnim()
    {
        taskImage.DOColor(Color.green, 1);
        taskName.text = "Görev Tamamlandı";
        taskIndex++;
        taskDescription.text = "Tebrikler " + taskIndex + ". görev tamamlandı bir sonraki görevin geliyor.";
        yield return new WaitForSeconds(7);
        taskImage.DOColor(Color.white, 1);
        taskName.text = tasks[taskIndex].taskName;
        taskDescription.text = tasks[taskIndex].taskDescription;

        if (taskIndex == 3)
        {
            GameEvents.Instance.OnAllBuldingCanBuild?.Invoke();

        }

        if (taskIndex == 5)
        {
            GameEvents.Instance.OnEnemySpawnWave?.Invoke(30);

            yield return new WaitForSeconds(15);

            taskImage.DOFade(0, 1);
            taskName.text = "";
            taskDescription.text = "";


        }



    }

    public void TaskControl3(Enums.ResourceTypes resourceType)
    {
        if (istasksCompleted[2])
        {
            return;
        }

        if (resourceType == Enums.ResourceTypes.Demir)
        {
            isIronMineCompleted = true;
        }
        else if (resourceType == Enums.ResourceTypes.Bakir)
        {
            isCopperMineCompleted = true;
        }

        if (isIronMineCompleted && isCopperMineCompleted)
        {
            tasks[0].isCompleted = true;
            istasksCompleted[0] = true;
            StartCoroutine(TaskComplatedAnim());

        }


    }

    public void TaskControl4()
    {
        if (istasksCompleted[3])
        {
            return;
        }

        tasks[taskIndex].isCompleted = true;
        istasksCompleted[taskIndex] = true;
        StartCoroutine(TaskComplatedAnim());
    }

    public void TaskControl5()
    {
        if (istasksCompleted[taskIndex] || !istasksCompleted[taskIndex - 1])
        {
            return;
        }


        tasks[taskIndex].isCompleted = true;
        istasksCompleted[taskIndex] = true;
        StartCoroutine(TaskComplatedAnim());
    }

}
