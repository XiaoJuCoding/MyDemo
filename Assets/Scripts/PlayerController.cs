using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchGroundDetection))]
public class PlayerController : MonoBehaviour
{
    public HealthBar healthBar;
    TouchGroundDetection touchGroundDetection;
    //设置动画变量
    Animator animator;
    //设置刚体变量
    Rigidbody2D rb2d;
    //设置玩家速度
    //行走速度
    public float walkSpeed = 5.0f;
    //跑步速度
    public float runSpeed = 8.0f;
    //跳跃力量
    public float jumpForce = 5.0f;
    //滑墙速度
    public float wallSlideSpeed = -1.0f;
    //跳墙冷却
    public float wallJumpCoolDown;
    //设置当前移动速度
    public float climbSpeed = 2f;
    [Header("闪避")]
    public bool isSilding=false;
    public float slideForce = 3f;
    public float slidetime = 0f;
    public float slideDuration = 0.3f;

    public float currentMoveSpeed { get
        {
            if (canMove)
            {
                if (IsMoving && !touchGroundDetection.IsOnWall)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        } }
    //设置用户输入移动变量
    Vector2 moveInput;

    //是否移动
    [SerializeField]
    [Header("移动")]
    private bool _isMoving = false;
    public bool IsMoving { get { 
            return _isMoving; 
        } private set {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        } 
    }

    //是否跑动
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning{ get {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
    }

    private bool _isclimb = false;
    public bool IsClimb { get
        {
            return _isclimb;
        } 
        private set
        {
            _isclimb = value;
            animator.SetBool(AnimationString.climb, value);
        }
    }

    //是否面朝右
    private bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } }

    //锁定是否能移动
    public bool canMove { get {
            return animator.GetBool(AnimationString.canMove);
        } }

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


    void Awake()
    {
        //获取对象组件
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchGroundDetection = GetComponent<TouchGroundDetection>();
    }


    private void Update()
    {
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        //角色移动
        if (wallJumpCoolDown>0.3f)
        {
            if (IsAlive && canMove)
            {
                rb2d.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb2d.velocity.y);
            }
        }else
        {
            wallJumpCoolDown += Time.deltaTime;
        }
        //设置Y轴速度
        animator.SetFloat(AnimationString.yVelocity, rb2d.velocity.y);
        //华强
        WallSlide();
        slide();
    }

    //玩家移动
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive && !PauseMenu.GameIsPaused)
        {
            IsMoving = moveInput != Vector2.zero;

            setFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    //设置面朝向
    private void setFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x>0&& !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x<0&& IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void slide()
    {
        if (isSilding && IsMoving)
        {
            if (slidetime <= slideDuration)
            {
                rb2d.AddForce(moveInput * slideForce, ForceMode2D.Impulse);
                slidetime += Time.fixedDeltaTime;
            }
            else
            {
                isSilding = false;
                slidetime = 0;
            }
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (IsMoving)
            {
                animator.SetTrigger(AnimationString.Slide);
                isSilding = true;
            }
        }
    }


    //跳跃
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //地面跳跃
            if (touchGroundDetection.IsGround && canMove)
            {
                Jump();
            }
            //墙壁跳跃
            else if (touchGroundDetection.IsOnWall)
            {
                WallJump();
            }
        }

    }

    //攀爬
    public void Climb(InputAction.CallbackContext context)
    {
        if (touchGroundDetection.IsOnWall && !touchGroundDetection.IsGround && context.started)
        {
            IsClimb = true;
        }
    }



    //攻击
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && !PauseMenu.GameIsPaused)
        {
            animator.SetTrigger(AnimationString.attack);
            rb2d.velocity = Vector2.zero;
        }
    }

    //受击
    public void OnHit(int damage,Vector2 konckBack)
    {
        GameObject currentObject = this.gameObject;
        Damage HealthBar = currentObject.GetComponent<Damage>();
        healthBar.SetPlayerHealthBar(HealthBar.Health);
        rb2d.velocity = new Vector2(konckBack.x, rb2d.velocity.y + konckBack.y);
    }


    //滑墙
    public void WallSlide()
    {
        if (IsClimb)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -climbSpeed, float.MaxValue));
            if (touchGroundDetection.IsGround || !touchGroundDetection.IsOnWall)
            {
                IsClimb = false;
            }
        }
    }

    //跳跃
    public void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        animator.SetTrigger(AnimationString.jump);
    }

    //墙跳
    public void WallJump()
    {
        if (IsClimb)
        {
            rb2d.velocity = new Vector2(-transform.localScale.x * 5, jumpForce);
            //animator.SetTrigger(AnimationString.jump);
            wallJumpCoolDown = 0;
        }
    }


    //重置
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
