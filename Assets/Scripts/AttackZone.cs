using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public List<Collider2D> attackTarget = new List<Collider2D>();
    Collider2D Collider2D;

    // Start is called before the first frame update
    void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackTarget.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackTarget.Remove(collision);
    }
}
