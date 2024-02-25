using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public HealthBar healthBar;
    TouchGroundDetection touchGroundDetection;
    Rigidbody2D rb2dKnight;
    Animator animator;
    public DetectionZone attackZone;
    public DetectionZone groundDetection;
    public float walkSpeed = 2.0f;
    public float walkStopRate = 0.05f;
    public GameObject dropItem;
    Transform playertransform;
    public enum WalkableDirection {Right,Left}

    private WalkableDirection _walkDirection;
    GameObject currentObject;
    private Vector2 walkDirectionVector=Vector2.right;

    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if(_walkDirection != value)
            {
                gameObject.transform.localScale *= new Vector2(-1, 1);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    private bool _hasTarget = false;

    public bool HasTarget { get { return _hasTarget; } set {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        } }

    public bool canMove { get 
        { 
            return animator.GetBool(AnimationString.canMove); 
        } }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationString.isAlive);
        }
    }

    public float AttackCoolDown { get {
            return animator.GetFloat(AnimationString.AttackCoolDown);
        } private set {
            animator.SetFloat(AnimationString.AttackCoolDown, MathF.Max(value,0));
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        rb2dKnight = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchGroundDetection = GetComponent<TouchGroundDetection>();
    }

    private void Start()
    {
        playertransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        HasTarget = attackZone.target.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        /*if(touchGroundDetection.IsGround && touchGroundDetection.IsOnWall)
        {
            GroundFlip ();
        }*/

        if (IsAlive && canMove && !PauseMenu.GameIsPaused){
            if (HasTarget)
            {          
                transform.position = Vector2.MoveTowards(transform.position, playertransform.position, walkSpeed * Time.deltaTime);
                Flip();
            }
           else
            {
                rb2dKnight.velocity = new Vector2(walkDirectionVector.x * walkSpeed, rb2dKnight.velocity.y);
                UpdateDirection();
            }
        }        
    }

    public void GroundFlip()
    {
        if (canMove)
        {
            if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
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

    public void OnHit(int damage, Vector2 konckBack)
    {
        GameObject currentObject = this.gameObject;
        Damage Health = currentObject.GetComponent<Damage>();
        healthBar.EnemyHealthFrame.SetActive(true);
        healthBar.SetMaxEnemyHealthBar(Health.MaxHealth);
        if (IsAlive)
        {
            healthBar.SetEnemyHealthBar(Health.Health);
            rb2dKnight.velocity = new Vector2(konckBack.x, rb2dKnight.velocity.y + konckBack.y);
            Flip();
        }
        else
        {
            healthBar.EnemyHealthFrame.SetActive(false);
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallLine")
        {
            healthBar.EnemyHealthFrame.SetActive(false);
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb2dKnight.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb2dKnight.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
}
