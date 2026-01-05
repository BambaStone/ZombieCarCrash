using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour
{
    public GameObject Target;
    void Update()
    {
        transform.LookAt(Target.transform);
    }
}
