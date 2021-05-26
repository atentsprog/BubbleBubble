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
                Debug.Assert(hit.transform != null , "만약 이 로그가 보인다면 벽 레이어 지정안한거다, 해결안되면 프로그래머 한테 문의");
                float maxX = hit.point.x;
                pos.x = Mathf.Min(pos.x, maxX);
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
            rigidbody2D.gravityScale = gravityScale;
            enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 버블이 터질만큼 만힝 붙어 있다면 터트리자.
        //if(collision 이 벽인가?)
        ////collision.contacts[0].point  와 나와의 거리를 확인하자. -> 특정 값보다 작다면 터트리자.
    }
}
