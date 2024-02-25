using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGroundDetection : MonoBehaviour
{
    public ContactFilter2D contactFilter;

    CapsuleCollider2D touchcol;


    RaycastHit2D[] groundHit2Ds=new RaycastHit2D[5];
    RaycastHit2D[] wallHit2Ds = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHit2Ds = new RaycastHit2D[5];

    Animator animator;

    public float groundCastdistance = 0.05f;
    public float wallCastdistance = 0.5f;
    public float ceilingCastdistance = 0.5f;

    //�Ƿ��ڵ�������
    [SerializeField]
    private bool _isGround;
    public bool IsGround { get 
        { return _isGround; } 
        private set
        {
            _isGround = value;
            animator.SetBool(AnimationString.isGrounded, value);
        }
     }


    //�Ƿ���ǽ������
    //lambda���ǽ�ڷ��򷽷�
    private Vector2 wallCheck => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall
    {
        get
        { return _isOnWall; }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationString.isOnWall, value);
        }
    }

    //�Ƿ��ڶ�������
    [SerializeField]
    private bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get
        { return _isOnCeiling; }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationString.isOnCeiling, value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        touchcol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGround = touchcol.Cast(Vector2.down, contactFilter, groundHit2Ds, groundCastdistance) > 0;
        IsOnWall = touchcol.Cast(wallCheck, contactFilter,wallHit2Ds, wallCastdistance) > 0;
        IsOnCeiling = touchcol.Cast(Vector2.up, contactFilter,ceilingHit2Ds, ceilingCastdistance) > 0;
    }
}
