using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class coinItem : MonoBehaviour
{
    public AudioClip pickUpCoin;
    public int valuable;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            CoinUI.CurrentCoinQuantity += valuable;
            AudioSource.PlayClipAtPoint(pickUpCoin, GameObject.Find("Virtual Camera").transform.position,1);
            Destroy(gameObject);
        }
    }

}
