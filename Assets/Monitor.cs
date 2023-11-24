using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

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


    public IEnumerator MonitorRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        inovatimLogo.DOFade(0, 1.5f);
        yield return new WaitForSeconds(1.5f);
        foreach (char letter in monitorMessage)
        {
            monitorText.text += letter;
            yield return new WaitForSeconds(0.02f);

        }
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.OnTutorialInfoEnded?.Invoke();
        playButton.transform.DOScale(1, 1.5f);
    }
}
