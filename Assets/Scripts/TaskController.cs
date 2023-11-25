using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TaskController : MonoBehaviour
{
    [SerializeField]
    TMP_Text taskDescription;

    [SerializeField]
    Image taskImage;

    [SerializeField]
    RectTransform taskImageRect;

    [SerializeField]
    List<Task> tasks;

    public int ironMineCount, copperMineCount, woodCutterCount;

    bool isIronMineCompleted, isCopperMineCompleted;

    int taskIndex = 0;

    bool[] istasksCompleted = new bool[6];

    bool isIronMinerBuilt = false;
    bool isCopperMinerBuilt = false;

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
        // taskImageRect width = 500

        taskImageRect.sizeDelta = new Vector2(tasks[taskIndex].taskDescription.Length * 18, taskImageRect.sizeDelta.y);
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

        taskDescription.text = "10 Adet Odun Topla" + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + woodCutterCount + "/10 ] " + "</color>";



        if (woodCutterCount >= 10)
        {
            tasks[0].isCompleted = true;
            istasksCompleted[0] = true;
            StartCoroutine(TaskComplatedAnim());
        }

    }


    //"10 Adet Demir [0] 10 Adet Bakır [0] Topla
    public void Task2Update(Enums.ResourceTypes resourceTypes)
    {

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


        taskDescription.text = "10 Adet Demir " + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + ironMineCount + "/10 ] " + "</color>" + "Topla\n10 Adet Bakır " + $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>" + " [" + copperMineCount + "/10 ] " + "</color>" + "Topla";


        if (ironMineCount >= 10 && copperMineCount >= 10)
        {
            tasks[1].isCompleted = true;
            istasksCompleted[1] = true;
            StartCoroutine(TaskComplatedAnim());
        }

    }
    // Demir Maden Sondajı [x] Bakır Maden Sondajı [x] Kur
    public void Task3Update(Enums.ResourceTypes resourceTypes)
    {
        if (istasksCompleted[2] || !istasksCompleted[1]  )
        {
            return;
        }

        if (resourceTypes == Enums.ResourceTypes.Demir)
        {
            if (isCopperMinerBuilt)
            {
                taskDescription.text = "Demir Maden Sondajı [✓]\nBakır Maden Sondajı [✓]";
            }
            else
            {
                taskDescription.text = "Demir Maden Sondajı [✓]\nBakır Maden Sondajı [X]";
            }

        }
        else
        {
            if (isIronMinerBuilt)
            {
                taskDescription.text = "Demir Maden Sondajı [✓]\nBakır Maden Sondajı [✓]";
            }
            else
            {
                taskDescription.text = "Demir Maden Sondajı [X]\nBakır Maden Sondajı [✓]";
            }
        }



    }

    IEnumerator TaskComplatedAnim()
    {
        taskImage.DOColor(Color.green, 1);
        taskDescription.text = tasks[taskIndex].taskEnd;
        taskImageRect.sizeDelta = new Vector2(tasks[taskIndex].taskEnd.Length * 20, taskImageRect.sizeDelta.y);
        taskIndex++;



        yield return new WaitForSeconds(3);
        taskImage.DOColor(Color.white, 1);
        if (taskIndex == 1 || taskIndex == 2)
        {
            int maxLengt = Mathf.Max(tasks[taskIndex].taskDescription.Length, tasks[taskIndex].taskDescription2.Length);

            taskImageRect.sizeDelta = new Vector2(maxLengt * 20, taskImageRect.sizeDelta.y);
            taskDescription.text = tasks[taskIndex].taskDescription + "\n" + tasks[taskIndex].taskDescription2;

        }
        else
        {
            taskImageRect.sizeDelta = new Vector2(tasks[taskIndex].taskDescription.Length * 20, taskImageRect.sizeDelta.y);
            taskDescription.text = tasks[taskIndex].taskDescription;
        }



        if (taskIndex == 3)
        {
            GameEvents.Instance.OnAllBuldingCanBuild?.Invoke();

        }

        if (taskIndex == 5)
        {
            GameEvents.Instance.OnEnemySpawnWave?.Invoke(30);

            yield return new WaitForSeconds(15);

            taskImage.DOFade(0, 1);
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
            tasks[2].isCompleted = true;
            istasksCompleted[2] = true;
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
