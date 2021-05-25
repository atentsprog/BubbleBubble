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

    float previousY;
    private void Update()
    {
        FireBubble();

        Jump();

        Move();
    }

    public GameObject bubbleGo;
    private void FireBubble()
    {
        //스페이스키 누르면 앞으로 버블 발사.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bubbleGo, bubblePosition.position, transform.rotation);
        }
    }

    public Transform bubblePosition;

    private void Move()
    {
        // AD, A왼쪽, D오른쪽
        float moveX = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1;
        Vector3 position = transform.position;
        position.x = position.x + moveX * speed;
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
        if (previousY > transform.position.y)
            collider.isTrigger = false;
        previousY = transform.position.y;

        // 점프
        if (Input.GetKeyDown(KeyCode.J))
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
            collider.isTrigger = true;
            isGround = false;
        }
    }
}
