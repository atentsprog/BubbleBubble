using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0.1f;

    new public Collider2D collider;
    new public Rigidbody2D rigidbody2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        Application.targetFrameRate = 60;
        collider = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool isGround = true;
    public float jumpForce = 10;

    private void Update()
    {
        FireBubble();

        Jump();

        Move();

        CheatKey();
    }

    bool useLongRange = false;
    bool useRapidFire = false;
    bool useFastShot = false;
    public bool UseLongRange
    {
        get { return useLongRange; }
        set {
            Debug.LogWarning("LongRange:" + value);
            useLongRange = value; }
    }

    public float fireSpeedNormal = 0.5f; // 발사후 다른 발사동안 발사키 막힘.
    public float fireSpeedRapid = 0.2f; // useRapidFire옵션 활성화 되면 적용될 발사 속도

    public bool UseRapidFire
    {
        get { return useRapidFire; }
        set
        {
            Debug.LogWarning("RapidFire:" + value); 
            useRapidFire = value; }
    }

    public bool UseFastShot
    {
        get { return useFastShot; }
        set
        {
            Debug.LogWarning("FastShot:" + value); 
            useFastShot = value; }
    }

    public float FireInterval {
        get { return UseRapidFire ? fireSpeedRapid : fireSpeedNormal; }
    }

    private void CheatKey()
    {
        // 숫자 1롱 레인지.
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseLongRange = !UseLongRange;

        // 숫자 2 Rapid Fire
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseRapidFire = !UseRapidFire;

        // 숫자 3 패스트 샷.
        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseFastShot = !UseFastShot;
    }

    public GameObject bubbleGo;

    public float nextEnableFireTime = 0;
    private void FireBubble()
    {
        if (Time.time > nextEnableFireTime)
        {
            //스페이스키 누르면 앞으로 버블 발사.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BubbleMissile bm = Instantiate(bubbleGo, bubbelSpawnPos.position, transform.rotation)
                    .GetComponent<BubbleMissile>();
                bm.Init(UseFastShot, UseLongRange);

                nextEnableFireTime = Time.time + FireInterval;
            }
        }
    }
    public Transform bubbelSpawnPos;
    public float maxY = 13f;
    public float minX = -12.3f, maxX = 12.3f;


    private void Move()
    {
        // AD, A왼쪽, D오른쪽
        float moveX = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        Vector3 position = transform.position;
        position.x = position.x + moveX * speed;
        position.x = Mathf.Max(minX, position.x);
        position.x = Mathf.Min(maxX, position.x);
        transform.position = position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") == false)
        {
            if (moveX != 0)
            {
                animator.Play("run");
                var rotate = transform.rotation;
                rotate.y = moveX < 0 ? 180 : 0;
                transform.rotation = rotate;
                //spriteRenderer.flipX = moveX > 0;
            }
            else
                animator.Play("idle");
        }
    }

    private void Jump()
    {
        // 낙하할때는 지면과 충돌하도록 isTrigger를 꺼주자.
        if (rigidbody2D.velocity.y < 0)
        {
            //rigidbody2D.velocity.y가 최대값 보다 크다면 천장을 뚫고 있으므로 물리를 꺼두자.(isTrriger = true)
            if (maxY > transform.position.y)
                collider.isTrigger = false;
        }

        if (rigidbody2D.velocity.y == 0) // 공중에서 점프를 막고 싶다.
        {
            // 방향위혹은 W키 누르면 점프 하자.
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                DoJump();
            }
        }
    }

    public void DoJump()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(new Vector2(0, jumpForce));
        collider.isTrigger = true; // 점프할때 벽을 뚫고 싶다.
    }
}
