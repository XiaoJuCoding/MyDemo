using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcctackSound : MonoBehaviour
{
    public AudioClip Attack1,Attack2;
    public void PlayAttack1Sound()
    {
        AudioSource.PlayClipAtPoint(Attack1, GameObject.Find("Virtual Camera").transform.position, 1);

    }

    public void PlayAttack2Sound()
    {
        AudioSource.PlayClipAtPoint(Attack2, GameObject.Find("Virtual Camera").transform.position, 1);

    }
}
