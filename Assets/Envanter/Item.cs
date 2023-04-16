using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject 
{
  public string itemName;
  public Sprite itemIcon;

  public Enums.ItemTypes itemType;
}

