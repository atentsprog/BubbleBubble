using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 확장함수 샘플, 클래스 이름(MyExtension)은 중요하지 않음(마음대로 바꿔도됨, 파일이름과 달라도됨, 다른 클래스 이름과 중복되지만 않으면 됨)
/// </summary>
static public class MyExtension
{
    /// <summary>
    /// 특정 방향에서 y축을 기준으로 회전한 방향 벡터 반환
    /// </summary>
    /// <param name="baseDirection">기준이 되는 벡터 ex)transform.forward</param>
    /// <param name="angle">-180 ~ 180</param>
    /// <returns></returns>
    static public Vector3 AngleToYDirection(this Vector3 baseDirection, float angle)
    {
        var quaternion = Quaternion.Euler(0, angle, 0);
        Vector3 newDirection = quaternion * baseDirection;

        return newDirection;
    }

    static public string ToNumber(this int value)
    {
        return $"{value:N0}";
    }

    /// <summary>
    /// 경로를 리턴하는 함수
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    static public string GetPath(this Transform t)
    {
        // 부모가 있으면 부모 경로와 경로 구분자를 넣는다.
        StringBuilder sb = new StringBuilder();
        GetParentPath(t, sb);
        return sb.ToString();

        void GetParentPath(Transform tr, StringBuilder sb)
        {
            if (tr.parent != null)
            {
                GetParentPath(tr.parent, sb);

                sb.Append(tr.parent.name);
                sb.Append(System.IO.Path.DirectorySeparatorChar);
            }

            sb.Append(tr.name);
        }
    }
}