using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버블 이동 방식
///     버블은 일부 벽을 통과함.
/// 이동이 끝나면 맵마다 끌어당겨지는 지점으로 이동
/// 
/// todo : 몬스터가 닿을시 몬스터 버블에 갖히게 하기
/// </summary>
public class BubbleMissile : MonoBehaviour
{
    new public Rigidbody2D rigidbody2D;
    new public Collider2D collider2D;
    public float gravityScale = -0.5f;
    IEnumerator Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        rigidbody2D.gravityScale = 0;

        for (int i = 0; i < moveFrame; i++)
        {
            transform.Translate(speed, 0, 0);
            yield return null;
        }
        rigidbody2D.gravityScale = gravityScale;

        // 중력장을 찾아 이동하자. -> 맵마다 달랐음.
        // 중력장이 1개 이상인 곳도 있었음.
        //// 가장가까운 중력장을 찾자. 
        //// 중력장을 바라보자
        /// 중력장이 밑에 있으면 그래비티를 양수로 바꾸자.
        /// 앞방향으로 힘을 가해서 이동시키자.

    }

    public float speed = 1;
    public int moveFrame = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision + "부딛힘");
    }
}
