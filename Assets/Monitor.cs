using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class Monitor : MonoBehaviour
{
    [SerializeField]
    TMP_Text monitorText;

    [SerializeField]
    string monitorMessage;

    [SerializeField]
    GameObject playButton;

    [SerializeField]
    Image inovatimLogo;

    public VideoPlayer Videoplayer;




    public IEnumerator MonitorRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Videoplayer.Play();
        yield return new WaitForSeconds(15f);

        GameEvents.Instance.OnTutorialInfoEnded?.Invoke();
        playButton.transform.DOScale(1, 1.5f);
    }
}
