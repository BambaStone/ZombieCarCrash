using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPos : MonoBehaviour
{
    public GameObject car;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = car.transform.position;
        //transform.rotation = new Quaternion(0, car.transform.rotation.y, 0, car.transform.rotation.w);
    }
}
