using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviDoor : MonoBehaviour //특정 오브젝트에 트리거 될때 네비게이션을 발동시키는 스크립트
{
    public GameObject BeforeNavi;//네비게이션을 받아올 오브젝트
    public GameObject BeforDoor;//네비게이션이 가리킬 지나왔던 문

    private void OnTriggerEnter(Collider other)//트리거가 발동되었을때
    {
        if(other.CompareTag("Car"))//트리거된 오브젝트의 태그가 "Car"일때
        {
            BeforeNavi.GetComponent<Navi>().Target = BeforDoor;//네비게이션의 타겟을 지나왔던 문으로 지정
            BeforeNavi.SetActive(true);//네비게이션 활성화
        }
    }
    
}
