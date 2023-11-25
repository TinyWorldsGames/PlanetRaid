using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Landing : MonoBehaviour
{
    [SerializeField]
    ParticleSystem fireParticles, smokeParticles;
    [SerializeField]
    Transform landingPoint;

    [SerializeField]
    AudioSource doorSound;

    [SerializeField]
    Image blackScreen;

    [SerializeField] Transform mainDoor;

   [SerializeField] GameObject[] openObjects;
   [SerializeField]  GameObject[] closeObjects;

   [SerializeField] GameObject userInterface;


    private void Start()
    {

        transform.DOMove(landingPoint.position, 4f);
        mainDoor.DOPunchRotation(new Vector3(0, 0, 2), 1f, 1, 2.5f).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(LandingRoutine());

    }

    void Init()
    {
        foreach (GameObject obj in openObjects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in closeObjects)
        {
            obj.SetActive(false);
        }

    }



    IEnumerator LandingRoutine()
    {
        int totalTime = 0;

        while (true)
        {
            fireParticles.Play();
            smokeParticles.Play();
            yield return new WaitForSeconds(0.5f);
            fireParticles.Stop();
            smokeParticles.Stop();
            yield return new WaitForSeconds(0.5f);
            totalTime++;
            if (totalTime == 3)
            {
                fireParticles.Stop();
                smokeParticles.Stop();
                doorSound.Play();
                blackScreen.DOFade(1, 1f);
                break;
            }
        }

        yield return new WaitForSeconds(1f);
        Init();

        blackScreen.DOFade(0, 1.5f).OnComplete(() =>
        {
            blackScreen.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(2f);
        userInterface.SetActive(true);


    }

}
