using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0.1f;

    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // AD, A왼쪽, D오른쪽
        float moveX = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1;
        Vector3 position = transform.position;
        position.x = position.x + moveX * speed * Time.deltaTime;
        transform.position = position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") == false)
        {
            if (moveX != 0)
                animator.Play("run");
            else
                animator.Play("idle");
        }
    }
}
