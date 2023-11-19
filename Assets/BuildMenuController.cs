using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BuildMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject buildTypeMenu, buildMenu;

    [SerializeField]
    List<BuildType> buildTypes;

    [SerializeField] List<Image> buttonImages;
    [SerializeField] BuildElement buildElementPrefab;

    List<BuildElement> buildElements = new List<BuildElement>();

    List<Tween> currentTweens = new List<Tween>();

    private void OnEnable()
    {
        GameEvents.Instance.OnBuildMenuOpened += OpenBuildTypeMenu;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnBuildMenuOpened -= OpenBuildTypeMenu;
    }

    void OpenBuildTypeMenu(bool isOpen)
    {
        foreach (Tween tween in currentTweens)
        {
            tween.Kill();
        }



        if (isOpen)
        {
            buildTypeMenu.SetActive(true);
            buildTypeMenu.transform.localScale = Vector3.zero;
            currentTweens.Add(buildTypeMenu.transform.DOScale(1, 0.25f));
            buildMenu.transform.localScale = Vector3.zero;
            buildMenu.SetActive(false);
        }

        else
        {
            foreach (Image buttonImage in buttonImages)
            {
                buttonImage.color = Color.white;
            }

            currentTweens.Add(buildMenu.transform.DOScale(0, 0.25f).OnComplete(() =>
           {
               buildMenu.SetActive(false);
           }));

            currentTweens.Add(buildTypeMenu.transform.DOScale(0, 0.25f).OnComplete(() =>
            {
                buildTypeMenu.SetActive(false);
            }));
        }

    }

    public void SelectBuildTypeMenu(int index)
    {
        buildElements.ForEach(x => Destroy(x.gameObject));
        buildElements.Clear();
        buildMenu.transform.localScale = Vector3.zero;
        currentTweens.Add(buildMenu.transform.DOScale(1, 0.25f));
        buildMenu.SetActive(true);

        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.color = Color.white;
        }

        buttonImages[index].color = Color.green;

        for (int i = 0; i < buildTypes[i].builds.Count; i++)
        {
            BuildElement newElement = Instantiate(buildElementPrefab, buildMenu.transform);

            newElement.buildType = buildTypes[index].builds[i];

            newElement.Init();

            buildElements.Add(newElement);

        }
    }

}


[System.Serializable]

public class BuildType
{
    public string name;

    public List<PlacedObjectTypeSO> builds;

}

[System.Serializable]
public class BuildResources
{
    public int resource1;
    public int resource2;
}