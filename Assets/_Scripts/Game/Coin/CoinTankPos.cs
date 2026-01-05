using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTankPos : MonoBehaviour //코인탱크의 위치를 차의 짐칸위치로 고정시키는 스크립트
{
    public GameObject Player;//플레이어의 차 오브젝트

    private void Update()
    {
        transform.position = Player.transform.position;//플레이어 차의 위치를 가져옴
        transform.rotation = Player.transform.rotation;//플레이어 차의 방향을 가져옴
    }
}
