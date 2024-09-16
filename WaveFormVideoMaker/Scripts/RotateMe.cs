using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    public float speed;

    public void FixedUpdate()
    {
        transform.Rotate(Vector3.up * speed);
    }
}
