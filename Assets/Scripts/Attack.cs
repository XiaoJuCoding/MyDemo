using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    HealthBar healthBar;
    public int attackDamage = 2;
    public Vector2 knockBack = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage damage = collision.GetComponent<Damage>();
        if(damage != null)
        {
            Vector2 deliveredKnockBakc = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            bool getHit=damage.Hit(attackDamage, deliveredKnockBakc);
            if (getHit)
            {
                Debug.Log(collision.name+"Уќжа" + attackDamage);
            }
        }
    }
}
