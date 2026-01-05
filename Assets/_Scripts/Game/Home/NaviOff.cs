using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviOff : MonoBehaviour
{
    public GameObject BeforeNavi;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            BeforeNavi.SetActive(false);
        }
    }
}
