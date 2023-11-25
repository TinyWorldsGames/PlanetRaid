using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeywordsController : MonoBehaviour
{
    bool w, a, s, d, space, shift;

    [SerializeField]
    Image wImage, aImage, sImage, dImage, spaceImage, shiftImage;

    [SerializeField] List<Image> images = new List<Image>();



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (w)
            {
                return;
            }
            else
                w = true;
            wImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (a)
            {
                return;
            }
            else
                a = true;
            aImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (s)
            {
                return;
            }
            else
                s = true;
            sImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (d)
            {
                return;
            }
            else
                d = true;
            dImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (space)
            {
                return;
            }
            else
                space = true;
            spaceImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (shift)
            {
                return;
            }
            else
                shift = true;
            shiftImage.DOColor(new Color(1, 0.5f, 0), 0.5f);

        }

        if (w && a && s && d && space && shift)
        {
            foreach (Image image in images)
            {
                image.DOFade(0, 1f);
            }
        }





    }

}
