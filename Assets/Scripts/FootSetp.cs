using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSetp : MonoBehaviour
{
    public AudioClip FootStep;
    // Start is called before the first frame update
    public void PlayFootStep()
    {
        AudioSource.PlayClipAtPoint(FootStep, GameObject.Find("Virtual Camera").transform.position, 1); 
    }

}
