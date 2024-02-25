using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEyeCantroller : MonoBehaviour
{
    public HealthBar healthBar;
    Rigidbody2D rb2dFlyingEye;
    Animator animator;
    Transform playertransform;
    public DetectionZone Detectionzone;
    public AttackZone attackZone;
    public float flyingSpeed=4f;
    public float flyingStopRate = 0.05f;
    Transform nextWayPoint;
    int wayPointNum = 0;
    public List<Transform> wayPoints;
    public GameObject dropItem;
    private float wayPointReachDistance=0.1f;



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
    private void Awake()
    {
        rb2dFlyingEye = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playertransform = GameObject.Find("Player").GetComponent<Transform>();
        nextWayPoint = wayPoints[wayPointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = Detectionzone.target.Count > 0;
        AttackTarget = attackZone.attackTarget.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(canMove && IsAlive && !PauseMenu.GameIsPaused)
        {
            if (AttackCoolDown == 0 && HasTarget)
            {
                transform.position = Vector2.MoveTowards(transform.position, playertransform.position, flyingSpeed * Time.deltaTime);
                Flip();
            }
            else if(!HasTarget)
            {
                Fly();
            }
        }
        else if(!IsAlive)
        {
            rb2dFlyingEye.velocity = new Vector2(0, rb2dFlyingEye.velocity.y);
        }
   
    }

    public void OnHit(int damage, Vector2 konckBack)
    {
        if (IsAlive == true)
        {
            GameObject currentObject = this.gameObject;
            Damage HealthBar = currentObject.GetComponent<Damage>();
            healthBar.EnemyHealthFrame.SetActive(true);
            healthBar.SetEnemyHealthBar(HealthBar.Health);
            rb2dFlyingEye.velocity = new Vector2(konckBack.x, rb2dFlyingEye.velocity.y + konckBack.y);
        }
        else
        {
            healthBar.EnemyHealthFrame.SetActive(false);
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }

    public void Flip()
    {
        if(gameObject.transform.position.x < playertransform.position.x)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
    }


    public void Fly()
    {
        //到下一个点的方向
        Vector2 DistanceToPoint = (nextWayPoint.position - transform.position).normalized;
        rb2dFlyingEye.velocity = DistanceToPoint * flyingSpeed;

        //检查是否到达下一个点
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);
        UpdateDirection();
        
        //如果到达下一个点执行
        if(distance <= wayPointReachDistance)
        {
            wayPointNum++;
            if(wayPointNum >= wayPoints.Count)
            {
                //回到初始点
                wayPointNum = 0;
            }
            nextWayPoint = wayPoints[wayPointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb2dFlyingEye.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb2dFlyingEye.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallLine")
        {
            healthBar.EnemyHealthFrame.SetActive(false);
        }
    }
}
