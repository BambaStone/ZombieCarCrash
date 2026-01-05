using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSawBlade : MonoBehaviour//원형톱날분쇄기의 톱날을 회전시키는 스크립트
{
    void Update()//매프레임마다 호출
    {
        transform.Rotate(new Vector3(0, 1000f*Time.deltaTime, 0));//초당 1000의 속도로 로컬 Y축을 기준으로 회전
    }
}
