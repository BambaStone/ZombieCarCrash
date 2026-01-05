using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour //플레이어 차의 코인탱크 안의 코인 스크립트
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))//코인이 코인탱크에 들어가지 못하고 바닥에 떨어졌을때
        {
            transform.localPosition = Vector3.zero;//로컬포지션 (0,0,0)으로 이동(코인탱크의 안쪽)
        }
    }
}
