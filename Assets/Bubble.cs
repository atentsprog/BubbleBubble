using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int moveForwardFrame = 6;
    public int currentFrame = 0;
    public float speed = 0.7f;
    new public Rigidbody2D rigidbody2D;
    public float gravityScale = -0.7f;
    // 앞쪽 방향으로 이동., 6프레임 움직이고 나서 위로 이동(중력에 의해)
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
    }

    public LayerMask wallLayer;
    // 벽 뚫는 현상 수정되었음, 
    // 벽과 이미 충돌한상태로 생성되면 충돌되지 않음 <- 이 상황에선 버블이 터져야함.
    private void FixedUpdate()
    {
        if (currentFrame++ < moveForwardFrame)
        {
            var pos = rigidbody2D.position;
            pos.x += (speed * transform.forward.z);

            // 버블이 앞으로 가고 있으면 최대 X값을 레이 쏘아서 찾자.
            // 뒤로가고 있으면 최소 x값을 레이 쏘아서 찾자.
            if (transform.forward.z > 0) // 앞으로 가고 있다.
            {
                //최대 x값 찾자.
                var hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 100f, wallLayer);
                Debug.Assert(hit.transform != null , "만약 이 로그가 보인다면 벽 레이어 지정안한거다, 해결안되면 프로그래머 한테 문의", transform);
                if (hit.transform)
                {
                    float maxX = hit.point.x;
                    pos.x = Mathf.Min(pos.x, maxX);
                }
            }
            else
            {
                var hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 100f, wallLayer);
                float minX = hit.point.x;
                pos.x = Mathf.Max(pos.x, minX);
            }

            rigidbody2D.position = pos;
        }
        else
        {
            state = State.FreeFly;
            rigidbody2D.gravityScale = gravityScale;
            enabled = false;
        }
    }

    // 버블이 총알인 상태 : 앞으로 빠르게 이동하는 중(몬스터 닿으면 몬스터 잡힘).
    // 버블이 자유롭게 이동하는 상태 : 플레이어가 닿으면 버블 터짐.
    // 버블이 터지고 있는 상태 : <- 필요 없는 상태.
    public enum State
    {
        FastMove,
        FreeFly,
        //Expolosion,
    }
    public State state = State.FastMove;
    private void OnTouchCoillision(Transform tr)
    {
        if (state == State.FreeFly)
        {
            if (tr.CompareTag("Player"))
            {
                // 플레이어.
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision:" + collision.transform.name);

        //버블이 플레이어에 닿으면 터트리자.
        OnTouchCoillision(collision.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger:" + collision.transform.name);
        OnTouchCoillision(collision.transform);
    }
}
