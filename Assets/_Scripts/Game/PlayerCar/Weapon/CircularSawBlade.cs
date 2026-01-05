using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSawBlade : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 1000f*Time.deltaTime, 0));
    }
}
