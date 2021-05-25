using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteRenderer : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
