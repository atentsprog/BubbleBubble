using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByCollider : MonoBehaviour
{
    public AttackByColliderMonster parent;
    public string attackType = "Attack";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.OnAttack(collision, attackType);
    }
}
