using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class TutorialScene : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    Image blackScreen;

    [SerializeField]
    Monitor monitor;

    [SerializeField]
    PlayerController playerController;
    [SerializeField] PlayerInputHandler playerInput;

    [SerializeField] Transform playerTargetPoint;

    [SerializeField]
    GameObject landingInfoText;

    bool canLanding = false;

  

    private void OnEnable()
    {
        GameEvents.Instance.OnTutorialInfoEnded += RedyForLanding;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnTutorialInfoEnded -= RedyForLanding;
    }



    private void Start()
    {
        StartCoroutine(CapsuleRoutine());

    }

    private void Update() {
        if (canLanding)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Landing();
            }
        }
    }

    void RedyForLanding()
    {
        canLanding = true;
        landingInfoText.SetActive(true);
    }

    public void Landing()
    {
        if (!canLanding)
        {
            return;
        }

        landingInfoText.transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            landingInfoText.SetActive(false);
        });

        canLanding = false;
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(1, 2f);
        SceneManager.LoadScene(2);

    }

    IEnumerator CapsuleRoutine()
    {
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        blackScreen.DOFade(0, 2f).OnComplete(() =>
        {
            blackScreen.gameObject.SetActive(false);
        });
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {


            monitor.StartCoroutine(monitor.MonitorRoutine());
            playerInput.SetCoursorActive(true);
            playerInput.cursorInputForLook = false;
            playerInput.cursorLocked = false;
            playerController.enabled = false;
            playerController.animator.SetFloat("Speed", 2.5f);
            playerController.transform.DOMove(playerTargetPoint.position, 0.75f).OnComplete(() =>
            {
                playerController.animator.SetFloat("Speed", 0);

                playerController.transform.DORotate(playerTargetPoint.rotation.eulerAngles, 1f);
            });




        }
    }
}
