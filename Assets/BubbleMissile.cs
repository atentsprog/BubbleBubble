using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 버블 이동 방식
///     버블은 일부 벽을 통과함.
/// 이동이 끝나면 맵마다 끌어당겨지는 지점으로 이동
/// 
/// todo : 몬스터가 닿을시 몬스터 버블에 갖히게 하기
/// </summary>
public class BubbleMissile : MonoBehaviour
{
    static List<BubbleMissile> Items = new List<BubbleMissile>();
    public Animator animator;
    new public Rigidbody2D rigidbody2D;
    new public Collider2D collider2D;
    public float gravityScale = -0.5f;
    public float randomX = 1;
    public float randomY = 1;
    public LayerMask wallLayer; // 벽과 충돌하는것으 확인하기 위해서 추가함.

    IEnumerator Start()
    {
        Items.Add(this);
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        rigidbody2D.gravityScale = 0;
        state = State.Fire;
        for (int i = 0; i < moveFrame; i++)
        {
            var pos = transform.position;
            pos.x += (speed * transform.forward.z);
            //x방향으로 갈 수 있는 최대 지점을 광선을 쏘아서 확인하자.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.forward.z, 0), 100f, wallLayer); //LayerMask.NameToLayer("Wall")
            if (hit.collider != null) // 충돌한 벽이 있다면 벽 충돌 지점까지만 이동하자.
            {
                if (transform.forward.z > 0)
                    pos.x = Mathf.Min(pos.x, hit.point.x);
                else
                    pos.x = Mathf.Max(pos.x, hit.point.x);
            }

            transform.position = pos;
            yield return null;
        }
        state = State.FreeFly;
        rigidbody2D.gravityScale = gravityScale;
        rigidbody2D.AddForce(new Vector2(Random.Range(-randomX, randomX), Random.Range(-randomY, randomY)));
        // 중력장을 찾아 이동하자. -> 맵마다 달랐음.
        // 중력장이 1개 이상인 곳도 있었음.
        //// 가장가까운 중력장을 찾자. 
        //// 중력장을 바라보자
        /// 중력장이 밑에 있으면 그래비티를 양수로 바꾸자.
        /// 앞방향으로 힘을 가해서 이동시키자.

    }

    public float speed = 1;
    public int moveFrame = 4;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " Trigger 부딛힘");
        OnTouch(other.transform);
        //플레이어 점프하면 trigger에 체크 되어서 여기로 온다.
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.transform.name + " Collision 부딛힘");
        OnTouch(other.transform);
    }

    private void OnTouch(Transform otherTr)
    {
        switch (state)
        {
            case State.Fire:
                if (otherTr.tag == "Enemy")
                {
                    state = BubbleMissile.State.Capture;

                    //적을 감추고
                    //풍선이미지를 바꾸자.
                    // 풍선 현재 타입을 캡쳐로 설정.
                    otherTr.SetParent(transform);
                    otherTr.localPosition = Vector3.zero;
                    otherTr.gameObject.SetActive(false);
                    caughtTarget = otherTr.gameObject;

                    // 갖힌 적 타입에 따라 플레이 해야하는 애니메이션 이름 구하자.(지금은 하드 코딩)
                    StartCoroutine(BubbleExplosionTimerCo("EnemyA"));
                }
                break;
            case State.FreeFly:
                {
                    if (otherTr.transform.tag == "Player")
                    {
                        //플레이어에게 부딪힌다면 터트리자.
                        //Destroy(gameObject);
                        //인근의 모든 버블 함게 터트리자.
                        DestroyNearBubble();
                    }
                }
                break;
        }
    }

    public float nearDistance = 4.2f;
    private void DestroyNearBubble()
    {
        List<BubbleMissile> destroyBubble = new List<BubbleMissile>();
        Vector3 explosionPoint = transform.position;
        FindNearBubble(destroyBubble, explosionPoint);

        destroyBubble.ForEach(x => x.ExplosionNearBubble());
    }

    private void ExplosionNearBubble()
    {
        // todo:잡은 몬스터가 있었다면 죽이자.

        Destroy(gameObject);
    }

    private void FindNearBubble(List<BubbleMissile> destroyBubble, Vector3 explosionPoint)
    {
        foreach (var item in Items)
        {
            if (destroyBubble.Contains(item))
                continue;
            float distace = Vector3.Distance(item.transform.position, explosionPoint);
            if (distace < nearDistance)
            {
                Vector3 _explosionPoint = item.transform.position;
                destroyBubble.Add(item);
                FindNearBubble(destroyBubble, _explosionPoint);
            }
        }
    }

    public GameObject caughtTarget; //catch잡은 겟

    public int[] explosionTimer;
    private IEnumerator BubbleExplosionTimerCo(string prefix)
    {
        for (int i = 1; i <= explosionTimer.Length; i++)
        {
            animator.Play(prefix + i);
            yield return new WaitForSeconds(explosionTimer[i-1]);
        }

        // 몬스터 해방.
        caughtTarget.transform.SetParent(null);
        caughtTarget.SetActive(true);

        // 버블 없애자.
        Destroy(gameObject);
    }

    public State state = BubbleMissile.State.Fire;

    public enum State
    {
        Fire,
        FreeFly,
        Capture,
        Explosion,
    }

    private void OnDestroy()
    {
        Items.Remove(this);
    }
}
