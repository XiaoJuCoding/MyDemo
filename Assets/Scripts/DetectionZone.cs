using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollider;
    public List<Collider2D> target = new List<Collider2D>();
    Collider2D collider2d;

    // Start is called before the first frame update
    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.Remove(collision);

        if(target.Count <= 0)
        {
            noCollider.Invoke();
        }
    }
}
