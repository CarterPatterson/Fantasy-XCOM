using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Selector selector;

    public Vector2Int gridPos;
    public Vector2Int gridTarget;

    public virtual void Init()
    {
        //selector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Selector>();
        //selector.selectables.Add(this);
    }
}
