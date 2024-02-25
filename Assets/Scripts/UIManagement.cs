using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManagement : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject gamesCanvas;

    private void Awake()
    {
        gamesCanvas = GameObject.Find("Canvas");
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamage.AddListener(CharacterCausesDamage);
        CharacterEvents.characterHealed.AddListener(CharacterRecovery);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamage.RemoveListener(CharacterCausesDamage);
        CharacterEvents.characterHealed.RemoveListener(CharacterRecovery);
    }

    public void CharacterCausesDamage(GameObject character,int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gamesCanvas.transform).
            GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterRecovery(GameObject character,int healthRecpvery)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gamesCanvas.transform).
            GetComponent<TMP_Text>();

        tmpText.text = healthRecpvery.ToString();
    }

    /*public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            #if(UNITY_EDITOR || DEVELOPMENT_BUILD)
                        Debug.Log(this.name + ":" + this.GetType() + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif
            #if(UNITY_EDITOR)
                        UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
                        Application.Quit();
            #elif(UNITY_WEBGL)
                        SceneManager.LoadScene("QuitScene")
            #endif
        }
    }*/
}
