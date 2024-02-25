using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class Shop : MonoBehaviour
{
    public bool canOpenShop = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (canOpenShop)
            {
                UIManager.Instance.OpenPanel(UIConst.LotteryPanel);
                PauseMenu.GameIsPaused = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            canOpenShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            canOpenShop = false;
        }
    }
}
