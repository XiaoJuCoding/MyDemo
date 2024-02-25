using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NightBorneCantroller : MonoBehaviour
{
    public HealthBar healthBar;
    Rigidbody2D rb2dNightBorne;
    Animator animator;
    Transform playertransform;
    GameObject limit;
    Light2D BgLight;
    public GameObject dropItem;

    public DetectionZone Detectionzone;
    public AttackZone attackZone;
    public float walkSpeed = 6.0f;

    private bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        }
    }


    private bool _attackTarget = false;

    public bool AttackTarget
    {
        get { return _attackTarget; }
        set
        {
            _attackTarget = value;
            animator.SetBool(AnimationString.attackTarget, value);
        }
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationString.isAlive);
        }
    }

    public float AttackCoolDown
    {
        get
        {
            return animator.GetFloat(AnimationString.AttackCoolDown);
        }
        private set
        {
            animator.SetFloat(AnimationString.AttackCoolDown, MathF.Max(value, 0));
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb2dNightBorne = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playertransform = GameObject.Find("Player").GetComponent<Transform>();
        limit = GameObject.Find("limit");
        BgLight = GameObject.Find("Light 2D").GetComponent<Light2D>();
        limit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Target();
        Attack();
    }

    private void FixedUpdate()
    {
        if (canMove && IsAlive && !PauseMenu.GameIsPaused)
        {
            if (AttackCoolDown == 0 && HasTarget)
            {
                transform.position = Vector2.MoveTowards(transform.position, playertransform.position, walkSpeed * Time.deltaTime);
                Flip();
            }
            else
            {
                rb2dNightBorne.velocity = Vector2.zero;
            }
        }
    }

    public void OnHit(int damage, Vector2 konckBack)
    {
        GameObject currentObject = this.gameObject;
        Damage Health = currentObject.GetComponent<Damage>();
        healthBar.EnemyHealthFrame.SetActive(true);
        healthBar.SetMaxEnemyHealthBar(Health.MaxHealth);
        if (IsAlive == true)
        {
            healthBar.SetEnemyHealthBar(Health.Health);
            rb2dNightBorne.velocity = new Vector2(konckBack.x, rb2dNightBorne.velocity.y + konckBack.y);
        }
        else
        {
            healthBar.EnemyHealthFrame.SetActive(false);
            BgLight.intensity = 1;
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }

    public void Flip()
    {
        if (gameObject.transform.position.x < playertransform.position.x)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb2dNightBorne.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb2dNightBorne.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void Target()
    {
        HasTarget = Detectionzone.target.Count > 0;
        if (HasTarget && IsAlive)
        {
            limit.SetActive(true);
        }
        else
        {
            limit.SetActive(false);
        }
    }

    public void Attack()
    {
        AttackTarget = attackZone.attackTarget.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
    }
}
