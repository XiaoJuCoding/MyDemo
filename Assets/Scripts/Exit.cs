using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    PauseMenu menu;
    [Header("进度条提示信息：")]
    public Text tips_Txt;
    public GameObject ExitPaenl;
    bool isCanIntoScene = false;
    // Start is called before the first frame update
    void Start()
    {
        isCanIntoScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isCanIntoScene)
        {
            SceneManager.LoadScene("MainMenu");
            PauseMenu.GameIsPaused = false;
            Time.timeScale = 1.0f;
            CoinUI.CurrentCoinQuantity = 0;
            PlayerPrefs.SetInt("PlayerCoin", 0);
        }
    }

    IEnumerator Close()
    {
        //tips_Txt.gameObject.SetActive(true);
        tips_Txt.text = "按下任意键回到主菜单...";

        tips_Txt.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tips_Txt.DOFade(1, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            ExitPaenl.SetActive(true);
            StartCoroutine(Close());
            isCanIntoScene = true;
        }
    }
}
