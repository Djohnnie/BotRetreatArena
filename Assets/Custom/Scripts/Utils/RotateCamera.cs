using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour
{

    public float rotationsPerMinute = 10.0f;

    void Update()
    {
        transform.Rotate(0f, 6.0f * rotationsPerMinute * Time.deltaTime, 0f);
    }
}