using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGravityFiled : MonoBehaviour
{
    public static List<BubbleGravityFiled> Items = new List<BubbleGravityFiled>();

    private void Awake()
    {
        Items.Add(this);
    }
    private void OnDestroy()
    {
        Items.Remove(this);
    }
}
