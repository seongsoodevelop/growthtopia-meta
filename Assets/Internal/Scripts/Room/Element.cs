using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Element : MonoBehaviour
{
    public int id { get; private set; }

    public void Init(int id, Vector3 position)
    {
        this.id = id;
        transform.position = position;
    }
}
