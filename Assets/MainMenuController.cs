using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject SettingsPanel,SoundOnOfButons, SoundOnOfButons1;
    
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);


    }
    public void SettingScreen()
    {
        if (SettingsPanel != null)
        {
            // Panelin aktif durumunu tersine çevir (açýksa kapat, kapalýysa aç)
            SettingsPanel.SetActive(!SettingsPanel.activeSelf);
        }


    }
    public void SoundOnOf()
    {
        if (SoundOnOfButons != null)
        {
            // Panelin aktif durumunu tersine çevir (açýksa kapat, kapalýysa aç)
            SoundOnOfButons.SetActive(!SoundOnOfButons.activeSelf);
            SoundOnOfButons.SetActive(!SoundOnOfButons1.activeSelf);
        }


    }
}
