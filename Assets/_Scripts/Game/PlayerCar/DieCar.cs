using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCar : MonoBehaviour//플레이어의 차가 죽었을때 소환되는 이펙트와 오브젝트를 관리하는 스크립트
{
    public GameObject Smoke;    //파괴후 연기이펙트의 오브젝트
    public GameObject Car;      //플레이어의 자동차 오브젝트
    private void OnEnable()// 해당 스크립트가 포함된 오브젝트가 활성화 되었을때
    {
        //해당 스크립트가 포함된 오브젝트에 폭발 이펙트가 활성화시 재생으로 포함되어있음
        StartCoroutine(ActiveSmoke());//연기 이펙트 활성화 함수 지연호출
        StartCoroutine(ReActiveCar());//플레이어의 자동차 재활성화 함수 지연호출
    }

    IEnumerator ActiveSmoke()//연기 이펙트 함수
    {
        yield return new WaitForSeconds(2f);//2초 지연
        Smoke.SetActive(true);//연기 이펙트 활성화
    }

    IEnumerator ReActiveCar()//플레이어 차의 재활성화 함수
    {
        yield return new WaitForSeconds(10f);//10초 지연
        Car.transform.position = Vector3.zero;//플레이어 차의 위치 초기화
        Car.transform.rotation = Quaternion.identity;//플에이어 차의 방향 초기화
        Car.GetComponent<PlayerController>().HP = Car.GetComponent<PlayerController>().MAXHP;//플레이어의 HP 최대치로 초기화
        StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin / 2; //월드코인의 2분에 1 제거
        Car.SetActive(true);//플레이어의 차 활성화
        Smoke.SetActive(false);//연기이펙트 비활성화
        gameObject.SetActive(false);//파괴된 차 오브젝트 비활성화
    }
}
