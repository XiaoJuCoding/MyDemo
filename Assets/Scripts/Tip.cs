using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    public GameObject TipBox;
    public float timerdisplay;
    // Start is called before the first frame update
    void Start()
    {
        TipBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerdisplay >= 0)
        {
            timerdisplay -= Time.deltaTime;
            if(timerdisplay < 0)
            {
                TipBox.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        timerdisplay = 3f;
        TipBox.SetActive(true);
    }
}
