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
    //���ö�������
    Animator animator;
    //���ø������
    Rigidbody2D rb2d;
    //��������ٶ�
    //�����ٶ�
    public float walkSpeed = 5.0f;
    //�ܲ��ٶ�
    public float runSpeed = 8.0f;
    //��Ծ����
    public float jumpForce = 5.0f;
    //��ǽ�ٶ�
    public float wallSlideSpeed = -1.0f;
    //��ǽ��ȴ
    public float wallJumpCoolDown;
    //���õ�ǰ�ƶ��ٶ�
    public float climbSpeed = 2f;
    [Header("����")]
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
    //�����û������ƶ�����
    Vector2 moveInput;

    //�Ƿ��ƶ�
    [SerializeField]
    [Header("�ƶ�")]
    private bool _isMoving = false;
    public bool IsMoving { get { 
            return _isMoving; 
        } private set {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        } 
    }

    //�Ƿ��ܶ�
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

    //�Ƿ��泯��
    private bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } }

    //�����Ƿ����ƶ�
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
        //��ȡ�������
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
        //��ɫ�ƶ�
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
        //����Y���ٶ�
        animator.SetFloat(AnimationString.yVelocity, rb2d.velocity.y);
        //��ǿ
        WallSlide();
        slide();
    }

    //����ƶ�
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

    //�����泯��
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


    //��Ծ
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //������Ծ
            if (touchGroundDetection.IsGround && canMove)
            {
                Jump();
            }
            //ǽ����Ծ
            else if (touchGroundDetection.IsOnWall)
            {
                WallJump();
            }
        }

    }

    //����
    public void Climb(InputAction.CallbackContext context)
    {
        if (touchGroundDetection.IsOnWall && !touchGroundDetection.IsGround && context.started)
        {
            IsClimb = true;
        }
    }



    //����
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && !PauseMenu.GameIsPaused)
        {
            animator.SetTrigger(AnimationString.attack);
            rb2d.velocity = Vector2.zero;
        }
    }

    //�ܻ�
    public void OnHit(int damage,Vector2 konckBack)
    {
        GameObject currentObject = this.gameObject;
        Damage HealthBar = currentObject.GetComponent<Damage>();
        healthBar.SetPlayerHealthBar(HealthBar.Health);
        rb2d.velocity = new Vector2(konckBack.x, rb2d.velocity.y + konckBack.y);
    }


    //��ǽ
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

    //��Ծ
    public void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        animator.SetTrigger(AnimationString.jump);
    }

    //ǽ��
    public void WallJump()
    {
        if (IsClimb)
        {
            rb2d.velocity = new Vector2(-transform.localScale.x * 5, jumpForce);
            //animator.SetTrigger(AnimationString.jump);
            wallJumpCoolDown = 0;
        }
    }


    //����
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
