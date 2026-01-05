using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEffect : MonoBehaviour//이펙트가 발동되고, 비활성화 시키는 스크립트
{
    // Start is called before the first frame update
    private void OnEnable()//이펙트가 활성화 될시에 호출
    {
        StartCoroutine(UnActive());//비활성화 함수를 지연호출
    }

    IEnumerator UnActive()//비활성화 함수
    {
        yield return new WaitForSeconds(1f);//1초후
        gameObject.SetActive(false);//이펙트 오브젝트 비활성화
    }
}
