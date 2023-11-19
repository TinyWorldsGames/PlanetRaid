using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{

    private void OnEnable()
    {
        GameManager.Instance.batteryCount++;

    }

    private void OnDisable()
    {
        GameManager.Instance.batteryCount--;
    }
}
