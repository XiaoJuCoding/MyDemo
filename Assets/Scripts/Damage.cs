using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageHit;
    Animator animator;
    [SerializeField]
    private int _maxHealth=10;
    [SerializeField]
    private int _health=10;
    [SerializeField]
    private bool _isAlive=true;
    [SerializeField]
    private bool godMode;
    [SerializeField]
    private float timeHit=0;
    [SerializeField]
    private float godModeTime=0.5f;
    public HealthBar healthBar;

    public int MaxHealth { 
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (timeHit > godModeTime)
        {
            godMode = false;
            timeHit = 0;
        }
        timeHit += Time.deltaTime;
    }

    public bool Hit(int damage,Vector2 konckBack)
    {
        if (IsAlive && !godMode)
        {
            Health -= damage;
            godMode = true;
            animator.SetTrigger(AnimationString.hit);
            damageHit?.Invoke(damage, konckBack);
            CharacterEvents.characterDamage.Invoke(gameObject, damage);
            return true;
        }return false;
    }

    public void Heal(int health)
    {
        if (IsAlive)
        {
            Health += health;
            Health = Mathf.Clamp(Health + health, 0, MaxHealth);
            Debug.Log(Health);
            healthBar.SetPlayerHealthBar(Health);
            CharacterEvents.characterHealed.Invoke(gameObject, health);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallLine")
        {
            IsAlive = false;
        }
    }
}
