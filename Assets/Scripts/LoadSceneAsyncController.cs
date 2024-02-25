using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class LoadSceneAsyncController : MonoBehaviour
{
    [Header("��Ч")]
    public AudioClip clearStage;
    [Header("�������ؽ�������")]
    public Slider scene_Slider;
    [Header("��������ʾ��")]
    public Text process_Txt;
    [Header("��������ʾ��Ϣ��")]
    public Text tips_Txt;
    public GameObject LoadUI;
    AsyncOperation _async;
    int sceneIndex = 0;
    [SerializeField] int level;

    //�Ƿ���Խ��볡��
    private bool isCanIntoScene = false;

    void Start()
    {

        isCanIntoScene = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            AudioSource.PlayClipAtPoint(clearStage, GameObject.Find("Virtual Camera").transform.position, 1);
            PauseMenu.GameIsPaused = true;
            PlayerPrefs.SetInt("SceneIndex", level);
            //PlayerPrefs.SetInt("PlayerCoin", CoinUI.CurrentCoinQuantity);
            LoadUI.SetActive(true);
            scene_Slider.value = 0;
            tips_Txt.text = "��Դ������...";
            StartCoroutine(LoadAsync_IE(sceneIndex));
        }
    }

    void Update()
    {

        if (Input.anyKeyDown && isCanIntoScene)
        {
            _async.allowSceneActivation = true;
            isCanIntoScene = false;
            PauseMenu.GameIsPaused = false;
        }
    }

    IEnumerator LoadAsync_IE(int sceneIndex)
    {
        sceneIndex = PlayerPrefs.GetInt("SceneIndex");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("��ǰ�ĳ�������Ϊ��" + sceneIndex);
        _async = SceneManager.LoadSceneAsync(sceneIndex);
        float nowProgress = 0;
        float endProgress = 0;
        _async.allowSceneActivation = false;

        while (_async.progress < 0.9f)
        {
            endProgress = _async.progress * 100f;

            while (nowProgress < endProgress)
            {
                ++nowProgress;
                scene_Slider.value = nowProgress / 100;
                process_Txt.text = (int)(nowProgress) + "%";
                yield return new WaitForEndOfFrame();
            }
        }

        endProgress = 100;
        while (nowProgress < endProgress)
        {
            ++nowProgress;
            scene_Slider.value = nowProgress / 100;
            process_Txt.text = (int)(nowProgress) + "%";
            yield return new WaitForEndOfFrame();
        }

        isCanIntoScene = true;
        //tips_Txt.gameObject.SetActive(true);
        tips_Txt.text = "�������������...";

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
}
