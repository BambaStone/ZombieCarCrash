using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviDoor : MonoBehaviour
{
    public GameObject BeforeNavi;
    public GameObject BeforDoor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            BeforeNavi.GetComponent<Navi>().Target = BeforDoor;
            BeforeNavi.SetActive(true);
        }
    }
    
}
