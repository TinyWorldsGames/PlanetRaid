using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deneme : MonoBehaviour
{
   [SerializeField]
   Material redMat,blueMat,whiteMat;

   int mat;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChangeMat();

        }
        
    }

  void  ChangeMat()
    {
        if(mat==0)
        {
            GetComponent<SpriteRenderer>().material=redMat;
            mat++;
        }
        else
        {
             GetComponent<SpriteRenderer>().material=blueMat;
             mat=0;
        }

    }
}
