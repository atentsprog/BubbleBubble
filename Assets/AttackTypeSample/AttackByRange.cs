using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackByRange : MonoBehaviour
{
    [System.Serializable]
    public class AttackInfo
    {
        public string attackName = "Attack";
        public float range = 3;
    }
    public List<AttackInfo> attacks;


    public Transform player;

    public float searchInterval = 1f;
    public float attackableDistance = 6f;
    private IEnumerator Start()
    {
        while (true)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < attackableDistance)
            {
                // 랜덤으로 공격할것을 정하던가, 공격 사정거리 안에 있는 공격 종류만 선택하던가 로직으로 선택하자
                var currentAttack = attacks[Random.Range(0, attacks.Count)]; 

                if(currentAttack.range > distance)
                    Debug.Log($"{currentAttack.attackName} 공격을 바로 하자");
                else
                    Debug.Log($"적 근처로 이동한다음 {currentAttack.attackName} 공격을 하자");
            }
            yield return new WaitForSeconds(searchInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < attacks.Count; i++)
        {
            var item = attacks[i];
            Gizmos.DrawWireSphere(transform.position, item.range);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackableDistance);
        
    }
}
