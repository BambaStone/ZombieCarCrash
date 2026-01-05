using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour//네비게이션이 특정 타겟을 바라보게 만드는 스크립트
{
    public GameObject Target;//네비게이션이 가리킬 타겟
    void Update()//매 프레임마다 반복
    {
        transform.LookAt(Target.transform); //네비게이션이 타겟을 바라보게 만들기
    }
}
