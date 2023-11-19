using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildElement : MonoBehaviour
{
    public Image buildImage;

    public PlacedObjectTypeSO buildType;

    public void Init()
    {
        buildImage.sprite = buildType.buildMenuImage;
    }

    public void SelectBuildType()
    {
        if(GameEvents.Instance.OnResourceControl.Invoke(buildType.buildResources))
        {
            GameEvents.Instance.OnNewBuildingSelected?.Invoke(buildType);

        }
    }

 

}
