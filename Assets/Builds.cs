using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class Builds :ScriptableObject
{
    public string name;
    public int price;
    public GameObject[] prefab;
    public PlacedObjectTypeSO placedObject;
}
