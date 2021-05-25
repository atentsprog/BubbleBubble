using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRenderer : MonoBehaviour
{
    void Start()
    {
        var r = GetComponent<Renderer>();
        r.enabled = false;
    }
}
