using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 0.02f;
    void Update()
    {
        this.transform.Rotate(0, 0, rotateSpeed);
    }
}
