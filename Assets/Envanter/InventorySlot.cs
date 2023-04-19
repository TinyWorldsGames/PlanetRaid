using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
   public Item item;
   public Image itemIcon;
   public Text itemCountText;

   private void OnEnable() 
   {
      UpdateSlot();
      
   }

   void UpdateSlot()
   {
      itemIcon.sprite = item.itemIcon;
      itemCountText.text = item.itemCount.ToString();
   }
}
