using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    new public Collider2D collider;
    new public Rigidbody2D rigidbody;
    public float gravitiScale = 1;

    public float speed = 0.1f;
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // 지형 만날때까지 아래로 떨어지자.
    IEnumerator MoveCo()
    { 
        Collider2D[] collider2Ds = new Collider2D[4];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        int collidCount = 0;
        while (collidCount == 0)
        {
            collidCount = rigidbody.OverlapCollider(contactFilter2D, collider2Ds);
            yield return null;
        }
        Debug.Log($"충돌한 충돌체 수 :{collider2Ds.Length}");


        // 무한히 좌우를 왔다 갔다하자.
        // todo:가끔 절벽쪽으로 점프 해야함.
        //오른쪽부터 이동하자. 끝에 땋을때까지 이동하자.
        //이전 위치랑 같으면 방향을 회전하자.
        float previousX = 0; 
        while (true)
        {
            previousX = transform.position.x;
            transform.Translate(speed, 0, 0);
            yield return null;
            if (Mathf.Abs( previousX - transform.position.x) < minimumMove)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
    public float minimumMove = 0.001f;

    private void OnEnable()
    {
        StartCoroutine(MoveCo());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool rightDirection = false;
        if (transform.position.x > collision.transform.position.x)
            rightDirection = true;

        //충돌체 방향에 따라 로테이션을 180도 돌려주자.
        var rotation = transform.rotation;
        rotation.y = rightDirection ? 180f : 0;
        transform.rotation = rotation;
    }
}
