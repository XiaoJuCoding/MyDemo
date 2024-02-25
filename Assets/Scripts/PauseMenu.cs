using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UIManager;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    AudioSource audioSource;

    private void Start()
    {
        audioSource=GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }


    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        audioSource.Play();
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenBag()
    {
        Debug.Log("打开了背包");
        UIManager.Instance.OpenPanel(UIConst.BagMenu);
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        CoinUI.CurrentCoinQuantity = 0;
        PlayerPrefs.SetInt("PlayerCoin",0);
    }
}
