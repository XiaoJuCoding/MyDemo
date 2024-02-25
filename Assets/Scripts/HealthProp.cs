using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProp : MonoBehaviour
{
    public int health = 5;
    public HealthBar healthBar;
    public GameObject chicken;
    Vector3 rotateSpeed = new Vector3(0, 180, 0);
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    public void Update()
    {
       transform.eulerAngles += rotateSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage damage = collision.GetComponent<Damage>();
        if (damage && damage.Health < damage.MaxHealth)
        {
            damage.Heal(health);
            Destroy(chicken);
        }
        else
        {
            Debug.Log("ÑªÁ¿ÒÑÂú");
        }
    }
}
