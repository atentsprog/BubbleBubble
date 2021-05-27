using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByColliderMonster : MonoBehaviour
{
    internal void OnAttack(Collider2D collision, string attackType)
    {
        Debug.Log($"{collision.transform.name}이 공격 범위에 들어왔다 {attackType}으로 때리자");
    }
}
