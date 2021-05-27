using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 앞으로 움직이자. 
    // 벽을 만나면 방향 전환.

    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }
    public float speed = 0.1f;

    RaycastHit2D[] hits = new RaycastHit2D[1];
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float groundCheclRayLength = 1.5f;
    public float wallCheclRayLength = 1.1f;
    
    private void Update()
    {
        Debug.Assert(groundLayer > 0, "그라운드 레이어를 설정해주세요");
        Debug.Assert(wallCheclRayLength > 0, "벽 레이어를 설정해주세요");
        // 절벽 체크
        int collideCount = collider2D.Raycast(new Vector2(transform.forward.z, -1), hits, groundCheclRayLength, groundLayer);
        if (collideCount == 0) // 바닥이 없다면 방향 바꾸기.
        {
            // 절벽이다.
            ChangeRotation();
        }

        // 벽 체크
        collideCount = collider2D.Raycast(new Vector2(transform.forward.z, 0), hits, wallCheclRayLength, wallLayer);
        if (collideCount > 0) // 벽이 1개 이상 있다면 방향 바꾸기.
        {
            // 절벽이다.
            ChangeRotation();
        }

        var pos = transform.position;
        pos.x += speed * transform.forward.z;
        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, new Vector2(transform.forward.z, -1).normalized * groundCheclRayLength);    // 땅 체크
        Gizmos.DrawRay(transform.position, new Vector2(transform.forward.z,  0).normalized * wallCheclRayLength);      // 벽 체크
    }

    private void ChangeRotation()
    {
        var rotation = transform.rotation;
        if (rotation.y == 0)
            rotation.y = 180;
        else
            rotation.y = 0;

        transform.rotation = rotation;
    }
}
