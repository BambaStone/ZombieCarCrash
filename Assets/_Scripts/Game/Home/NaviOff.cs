using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviOff : MonoBehaviour //특정 오브젝트에 트리거 될때 네비게이션을 비활성화하는 스크립트
{
    public GameObject BeforeNavi;//비활성화할 네비게이션

    private void OnTriggerEnter(Collider other)//해당 스크립트가 포함된 오브젝트가 트리거가 발동될때
    {
        if (other.CompareTag("Car"))//발동된 트리거의 오브젝트의 태그가 "Car"일때
        {
            BeforeNavi.SetActive(false);//네비게이션 비활성화
        }
    }
}
